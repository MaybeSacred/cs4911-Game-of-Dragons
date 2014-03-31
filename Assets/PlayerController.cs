using UnityEngine;
using System.Collections;

/// <summary>
/// Takes user input and controls the player object.
/// Controls moving, jumping, attacking, dying, etc.
/// </summary>
public class PlayerController : GameBehaviour, IResettable 
{
	public static readonly int FRAGMENTS_PER_GEM = 4;

	public CameraScript theCamera;
	public Transform realTransform;
	public ParticleSystem flames;
	public Transform graphics;

	public int health{get; private set;}
	public int maxHealth;
	public int currentJumpNumber;
	public int totalJumps;
	public float jumpStrength;
	public float repeatedJumpStrengthFalloff;
	
	public float airAcceleration;
	public float groundAcceleration;
	public float maxSpeed;
	public float rotationSpeed;
	public float friction;

	public float playerPushForce;
	public float groundSkinWidth;
	private bool isGrounded;
	public float minSpeed;
	public float iceDrag;
	
	public float startFlameEmissionRate;
	private float flameStartSize;
	public float flameDuration;
	private float flameTimer;
	public float flameRechargeSpeed;
	public Transform fireCone;

	public float maxAttackStrength;

	private Vector3 oldPosition;

	public int gems;
	public int smallGems;
	public int coins;

	private bool isOnIcyGround;

	private Vector3 resetPosition;
	private Vector3 resetRotation;
	private Vector3 resetVelocity;
	private Vector3 resetAngularVelocity;
	private int resetGems;
	private int resetSmallGems;
	private int resetCoins;


	override protected void Start()
	{
		base.Start ();

		health = maxHealth;
		oldPosition = rigidbody.position;
		startFlameEmissionRate = flames.emissionRate;
		flameStartSize = flames.startSize;
		SaveState ();
	}

	void Update()
	{
		PlayerAttack ();
		PlayerControlForces();
	}

	void FixedUpdate () 
	{

	}

	void OnCollisionEnter(Collision theCollision)
	{

	}

	void OnTriggerEnter(Collider other)
	{
		
	}

	private void PlayerControlForces()
	{
		if (Input.GetKeyDown ("k"))
			GameOver ();  // instant kill for debugging

		UpdateGroundedState();
		if(!isOnIcyGround)
		{
			if (Vector3.Distance (oldPosition, rigidbody.position) < minSpeed)
				rigidbody.position = oldPosition;  // prevent small movements from slopes
		}
		if (isGrounded)
			currentJumpNumber = 0;
		float verticalInput = Input.GetAxisRaw("Vertical");
		float horizontalInput = Input.GetAxisRaw("Horizontal");

		Vector2 controlVector = new Vector2(
			theCamera.transform.forward.x * verticalInput + theCamera.transform.right.x * horizontalInput, 
			theCamera.transform.forward.z * verticalInput + theCamera.transform.right.z * horizontalInput
		);
		controlVector = controlVector.normalized;
		if (!isGrounded)
			controlVector = new Vector2 (controlVector.x * airAcceleration, controlVector.y * airAcceleration);

		addVelocityX (controlVector.x * groundAcceleration);
		addVelocityZ (controlVector.y * groundAcceleration);

		if(Input.GetKeyDown(Config.keyJump) && currentJumpNumber < totalJumps)
		{
			setVelocityY(jumpStrength * Mathf.Pow(repeatedJumpStrengthFalloff, currentJumpNumber));
			currentJumpNumber++;
			isGrounded = false;
		}

		Vector3 xzSpeed = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
		float xzMagnitude = xzSpeed.magnitude;
		if(xzMagnitude > maxSpeed) 
		{
			setVelocityX ( maxSpeed / xzMagnitude * rigidbody.velocity.x );
			setVelocityZ ( maxSpeed / xzMagnitude * rigidbody.velocity.z );
		}

		if (isGrounded && verticalInput == 0 && horizontalInput == 0 && !isOnIcyGround) 
		{
			setVelocityX (0);  // player shouldn't slide when on ground
			setVelocityZ (0);
		}
		else if(isOnIcyGround)
		{
			rigidbody.velocity *= iceDrag;
		}
		if(verticalInput != 0 || horizontalInput != 0)
			realTransform.forward = Vector3.RotateTowards(realTransform.forward, new Vector3(controlVector.x, 0, controlVector.y), rotationSpeed*Time.deltaTime, 0);
		oldPosition = rigidbody.position;
	}

	void PlayerAttack()
	{
		if(Input.GetMouseButton(0) || Input.GetKey(Config.keyBreath))
		{
			fireCone.collider.enabled = true;
			flames.emissionRate = startFlameEmissionRate;
			flames.startSize = Mathf.Lerp(flameStartSize, 0, flameTimer / flameDuration);
			if(flameTimer < flameDuration)
			{
				flameTimer += Time.deltaTime;
			}
		}
		else
		{
			fireCone.collider.enabled = false;
			flames.emissionRate = 0;
			if(flameTimer > 0)
			{
				flameTimer -= flameRechargeSpeed * Time.deltaTime;
				if(flameTimer <= 0)
				{
					flameTimer = 0;
				}
			}
		}
	}
	
	/// <returns>A value between 0 and 1 representing the visual strength of the flame.</returns>
	public float GetFlameScale()
	{
		return (flameDuration-flameTimer) / flameDuration;
	}
	
	/// <returns>A value representing how much damage the flame can do at the current time.</returns>
	public float GetAttackDamage()
	{
		return GetFlameScale() * maxAttackStrength;
	}

	private void UpdateGroundedState()
	{
		RaycastHit hit;
		isGrounded = Physics.Raycast(
			new Ray(rigidbody.position, -Vector3.up), 
		    out hit, 
			groundSkinWidth + collider.bounds.extents.y,
		    ~(1<<2 | 1<<8)
		) && Vector3.Dot(hit.normal, Vector3.up) > .5;  // restrict ground to less than 45 deg angles
		if(hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Icy"))
		{
			isOnIcyGround = true;
		}
		else
		{
			isOnIcyGround = false;
		}
	}

	/// <summary>
	/// Called when player is dead. Tells world to reset state.
	/// </summary>
	public void GameOver()
	{
		WorldScript.reset();
	}

	/// <summary>
	/// Adds deltaHealth to current health and kills player if health is less than or equal to 0.
	/// </summary>
	/// <param name="deltaHealth">Amount to change health by.</param>
	public void HealthChange(int deltaHealth)
	{
		health += deltaHealth;
		if(health <=0)
		{
			health = 0;
			GameOver();
		}
		else if(health > maxHealth)
		{
			health = maxHealth;
		}
	}

	/// <summary>
	/// Increments the gems.
	/// </summary>
	public void incrementGems()
	{
		gems++;
	}

	/// <summary>
	/// Increments the small gems.
	/// </summary>
	public void incrementSmallGems()
	{
		smallGems++;
		if (smallGems == FRAGMENTS_PER_GEM) 
		{
			smallGems = 0;
			gems++;
		}
	}

	/// <summary>
	/// Adds to the coint count.
	/// </summary>
	/// <param name="amt">Number of coins to add.</param>
	public void addCoins(int amt)
	{
		coins += amt;
	}

	/// <seealso cref="IResettable"/>
	public void SaveState()
	{
		resetPosition = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
		resetRotation = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
		resetVelocity = new Vector3(transform.rigidbody.velocity.x, transform.rigidbody.velocity.y, transform.rigidbody.velocity.z);
		resetAngularVelocity = new Vector3(transform.rigidbody.angularVelocity.x, transform.rigidbody.angularVelocity.y, transform.rigidbody.angularVelocity.z);;
		resetGems = gems;
		resetSmallGems = smallGems;
		resetCoins = coins;
	}

	/// <seealso cref="IResettable"/>
	public void Reset()
	{
		transform.position = resetPosition;
		transform.eulerAngles = resetRotation;
		transform.rigidbody.velocity = resetVelocity;
		transform.rigidbody.angularVelocity = resetAngularVelocity;
		gems = resetGems;
		smallGems = resetSmallGems;
		coins = resetCoins;
		health = maxHealth;
	}

	//----------
	// Velocity Functions

	void setVelocityX(float vx)
	{
		rigidbody.velocity = new Vector3 (vx, rigidbody.velocity.y, rigidbody.velocity.z);
	}

	void setVelocityY(float vy)
	{
		rigidbody.velocity = new Vector3 (rigidbody.velocity.x, vy, rigidbody.velocity.z);
	}

	void setVelocityZ(float vz)
	{
		rigidbody.velocity = new Vector3 (rigidbody.velocity.x, rigidbody.velocity.y, vz);
	}

	void setVelocity(float vx, float vy, float vz)
	{
		rigidbody.velocity = new Vector3 (vx, vy, vz);
	}

	void addVelocityX(float dvx)
	{
		setVelocityX(rigidbody.velocity.x + dvx);
	}
	
	void addVelocityY(float dvy)
	{
		setVelocityY(rigidbody.velocity.y + dvy);
	}
	
	void addVelocityZ(float dvz)
	{
		setVelocityZ(rigidbody.velocity.z + dvz);
	}

	void addVelocity(float dvx, float dvy, float dvz)
	{
		setVelocity (rigidbody.velocity.x + dvx, rigidbody.velocity.y + dvy, rigidbody.velocity.z + dvz);
	}
}
