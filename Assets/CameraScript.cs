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

	public float trueZoomSpeed;

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
		trueCameraOffset -= Input.GetAxisRaw("Mouse ScrollWheel")*Time.deltaTime*trueZoomSpeed;
		trueCameraOffset = Mathf.Clamp(trueCameraOffset, minZoomCameraOffset, maxZoomCameraOffset);
		RaycastHit hit;
		if(Physics.Raycast(WorldScript.thePlayer.transform.localPosition, -transform.forward, out hit, maxZoomCameraOffset, 1))
		{
			if(hit.distance < trueCameraOffset)
			{
				attemptedCameraOffset = Mathf.Lerp(attemptedCameraOffset, -trueCameraOffset+hit.distance-.5f, attemptedZoomSpeed * Time.deltaTime);
			}
			else
			{
				attemptedCameraOffset = Mathf.Lerp(attemptedCameraOffset, 0, attemptedZoomSpeed * Time.deltaTime);
			}
		}
		else
		{
			attemptedCameraOffset = Mathf.Lerp(attemptedCameraOffset, 0, attemptedZoomSpeed * Time.deltaTime);
		}
		currentCameraOffset = Mathf.Lerp(currentCameraOffset, trueCameraOffset + attemptedCameraOffset, Time.deltaTime*zoomSpeed);
		// set camera based on rotation
		transform.position = playerCharacter.transform.position - transform.forward * currentCameraOffset;
	}

	public void SaveState()
	{
		resetPosition = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
		resetRotation = new Vector3 (transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
	}
	public void Reset()
	{
		transform.position = resetPosition;
		transform.eulerAngles = resetRotation;
	}
}
