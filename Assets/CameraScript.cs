using UnityEngine;
using System.Collections;

public class CameraScript : GameBehaviour, IResettable
{
	public PlayerController playerCharacter;
	public Transform playerGraphics;
	public int currentCharacter;

	public Vector2 mousePos;
	public Vector2 mouseSensitivity;

	public float movingRotationSpeed;
	public float stationaryRotationSpeed;
	
	public float yAxisUpperAngleBound, yAxisLowerAngleBound;

	private float currentCameraOffset, attemptedCameraOffset, trueCameraOffset;
	public float attemptedZoomSpeed;
	public float zoomSpeed;
	public float minZoomCameraOffset, maxZoomCameraOffset;

	public float scrollSpeed;

	private Vector3 resetPosition;

	private bool isColliding;

	private Vector3 resetRotation;

	override protected void Start()
	{
		base.Start ();

		yAxisUpperAngleBound += 360;
		mousePos = new Vector2();
		attemptedCameraOffset = (maxZoomCameraOffset+minZoomCameraOffset)/2;

		SaveState ();
	}

	void Update () 
	{
		Screen.lockCursor = true;
		psychonautStyle();
	}

	void psychonautStyle()
	{
		mousePos.x = Input.GetAxis("Mouse X");
		mousePos.y = Input.GetAxis("Mouse Y");

		// rotate vertically
		if (transform.eulerAngles.x + mousePos.y * mouseSensitivity.y < yAxisUpperAngleBound && transform.eulerAngles.x + mousePos.y * mouseSensitivity.y > yAxisLowerAngleBound)
		{
			if(transform.eulerAngles.x + mousePos.y * mouseSensitivity.y > 180)
			{
				transform.eulerAngles = new Vector3 (yAxisUpperAngleBound, transform.eulerAngles.y + mousePos.x * mouseSensitivity.x, transform.eulerAngles.z);
			}
			else
			{
				transform.eulerAngles = new Vector3 (yAxisLowerAngleBound, transform.eulerAngles.y + mousePos.x * mouseSensitivity.x, transform.eulerAngles.z);
			}
		}
		else
		{
			transform.eulerAngles = new Vector3 (transform.eulerAngles.x + mousePos.y * mouseSensitivity.y, transform.eulerAngles.y + mousePos.x * mouseSensitivity.x, transform.eulerAngles.z);
		}

		// rotate to make player run in circle
		Vector3 xzSpeed = new Vector3(playerCharacter.rigidbody.velocity.x, 0, playerCharacter.rigidbody.velocity.z);
		float xzDist = Mathf.Sqrt (Mathf.Pow (transform.position.x - playerCharacter.transform.position.x, 2) + Mathf.Pow (transform.position.z - playerCharacter.transform.position.z, 2));
		float rightSpeed = Vector3.Dot (xzSpeed, transform.right);
		if (xzDist == 0)
			xzDist = .0001f;  // prevent divide by zero
		float deltaAngle = rightSpeed / xzDist;
		//transform.RotateAround (Vector3.zero, Vector3.up, deltaAngle);
		if(isColliding)
		{
			attemptedCameraOffset -= Time.deltaTime;
		}
		else
		{
			attemptedCameraOffset += Time.deltaTime;
			if(attemptedCameraOffset >= 0)
			{
				attemptedCameraOffset = 0;
			}
		}
		/*RaycastHit hit;
		if(Physics.Raycast(new Vector3(Util.player.transform.localPosition.x, Util.player.transform.localPosition.y + cameraOffset.y, Util.player.transform.localPosition.z), new Vector3(-transform.forward.x, 0, -transform.forward.z).normalized, out hit, 2*startZ, Util.PLAYERWEAPONSIGNORELAYERS))
		{
			if(hit.distance < startZ)
			{
				cameraOffset.x = Mathf.Lerp(cameraOffset.x, hit.distance-cameraRaycastOffset, lerpthThpeed*Time.deltaTime);
			}
			else
			{
				cameraOffset.x = Mathf.Lerp(cameraOffset.x, startZ, lerpthThpeed*Time.deltaTime);
			}
		}
		else
		{
			cameraOffset.x = Mathf.Lerp(cameraOffset.x, startZ, lerpthThpeed*Time.deltaTime);
		}*/
		trueCameraOffset -= Input.GetAxisRaw("Mouse ScrollWheel")*Time.deltaTime*attemptedZoomSpeed;
		trueCameraOffset = Mathf.Clamp(trueCameraOffset, minZoomCameraOffset, maxZoomCameraOffset);
		currentCameraOffset = Mathf.Lerp(currentCameraOffset, trueCameraOffset + attemptedCameraOffset, Time.deltaTime*zoomSpeed);
		// set camera based on rotation
		transform.position = playerCharacter.transform.position - transform.forward * currentCameraOffset;
	}

	public void SaveState()
	{
		resetPosition = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
		resetRotation = new Vector3 (transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
	}
	void OnCollisionStay(Collision other)
	{
		isColliding = true;
	}
	void OnCollisionExit()
	{
		isColliding = false;
	}
	public void Reset()
	{
		transform.position = resetPosition;
		transform.eulerAngles = resetRotation;
	}
}
