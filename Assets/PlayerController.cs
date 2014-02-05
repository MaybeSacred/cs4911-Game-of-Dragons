using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
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
	
	public float speed;
	public float maxSpeed;
	public float rotationSpeed;
	public float friction;

	public float playerPushForce;
	public float groundSkinWidth;
	private bool isGrounded;
	public float minSpeed;

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

	bool isOnIcyGround;

	void Start () 
	{
		health = maxHealth;
		oldPosition = rigidbody.position;
		startFlameEmissionRate = flames.emissionRate;
		flameStartSize = flames.startSize;
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
		switch (other.gameObject.tag) 
		{
		case "Gem":
			Destroy (other.gameObject);
			gems++;
			break;

		case "SmallGem":
			Destroy (other.gameObject);
			smallGems++;
			break;

		case "Coin":
			Destroy (other.gameObject);
			coins++;
			break;
		}
	}

	private void PlayerControlForces()
	{
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
		addVelocityX (controlVector.x * speed);
		addVelocityZ (controlVector.y * speed);

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
		if(!isOnIcyGround)
		{
			if(verticalInput != 0 || horizontalInput != 0)
				realTransform.forward = Vector3.RotateTowards(realTransform.forward, new Vector3(controlVector.x, 0, controlVector.y), rotationSpeed*Time.deltaTime, 0);
		}
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
			}
		}
	}

	public float getFlameScale()
	{
		if (flameStartSize == 0)
			return 0;
		else
			return flames.startSize / flameStartSize;
	}

	public float getAttackDamage()
	{
		return getFlameScale() * maxAttackStrength;
	}

	private void UpdateGroundedState()
	{
		RaycastHit hit;
		isGrounded = Physics.Raycast(
			new Ray(rigidbody.position, -Vector3.up), 
		    out hit, 
			groundSkinWidth + transform.localScale.y,
		    ~(1<<2 | 1<<8)
		);
		if(hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Icy"))
		{
			isOnIcyGround = true;
		}
		else
		{
			isOnIcyGround = false;
		}
	}
	public void GameOver()
	{
		
	}
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
