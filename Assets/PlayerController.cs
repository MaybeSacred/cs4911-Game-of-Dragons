using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	public CameraScript theCamera;
	public Transform realTransform;
	public Transform graphics;

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

	private Vector3 oldPosition;

	void Start () 
	{
		oldPosition = rigidbody.position;
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

	void OnControllerColliderHit(ControllerColliderHit hit) 
	{
		if(hit.transform.tag.Equals("Drum"))
		{
			DrumRoller drum = hit.transform.parent.GetComponent<DrumRoller>();
			Vector3 offset = transform.position - drum.transform.position;
			offset = Vector3.Cross(drum.transform.forward, offset);
		}
		else if(hit.transform.tag.Equals("MovableByPlayer"))
		{
			hit.rigidbody.AddForce((hit.transform.position - transform.position).normalized*playerPushForce);
		}
		else if(hit.transform.tag.Equals("MovingBlock"))
		{
			
		}
	}

	private void PlayerControlForces()
	{
		if (Vector3.Distance (oldPosition, rigidbody.position) < .05)
			rigidbody.position = oldPosition;  // prevent small movements from slopes

		bool isGrounded = getIfGrounded();

		if (isGrounded)
			currentJumpNumber = 0;

		float verticalInput = Input.GetAxisRaw("Vertical");
		float horizontalInput = Input.GetAxisRaw("Horizontal");

		Vector2 controlVector = new Vector2(
			theCamera.transform.forward.x * verticalInput + theCamera.transform.right.x * horizontalInput, 
			theCamera.transform.forward.z * verticalInput + theCamera.transform.right.z * horizontalInput
		);
		addVelocityX (controlVector.x * speed);
		addVelocityZ (controlVector.y * speed);

		if(Input.GetKeyDown(Config.keyJump) && currentJumpNumber < totalJumps)
		{
			setVelocityY(jumpStrength * Mathf.Pow(repeatedJumpStrengthFalloff, currentJumpNumber));
			currentJumpNumber++;
			isGrounded = false;
		}

		Vector3 xzSpeed = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
		float horizontalSpeed = xzSpeed.magnitude;
		if (horizontalSpeed > maxSpeed) 
		{
			setVelocityX ( maxSpeed / horizontalSpeed * rigidbody.velocity.x );
			setVelocityZ ( maxSpeed / horizontalSpeed * rigidbody.velocity.z );
		}

		if (isGrounded && verticalInput == 0 && horizontalInput == 0) 
		{
			setVelocityX (0);  // player shouldn't slide when on ground
			setVelocityZ (0);
		}

		if(verticalInput != 0 || horizontalInput != 0)
			realTransform.forward = Vector3.RotateTowards(realTransform.forward, new Vector3(controlVector.x, 0, controlVector.y), rotationSpeed*Time.deltaTime, 0);
	
		oldPosition = rigidbody.position;
	}

	void PlayerAttack()
	{

	}

	bool getIfGrounded()
	{
		RaycastHit hit;
		bool raycastResult = Physics.Raycast(
			new Ray(transform.position, -Vector3.up), 
		    out hit, 
			groundSkinWidth + transform.localScale.y,
		    ~(1<<2 | 1<<8)
		);

		return raycastResult;
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
