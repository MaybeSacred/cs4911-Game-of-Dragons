using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour 
{
	public PlayerController playerCharacter;
	public Transform playerGraphics;
	public int currentCharacter;

	public Vector2 mousePos;
	public Vector2 mouseSensitivity;

	public float movingRotationSpeed;
	public float stationaryRotationSpeed;

	public float yAxisUpperAngleBound, yAxisLowerAngleBound;

	public Vector3 cameraOffset;
	public float minCameraOffset, maxCameraOffset;

	public float scrollSpeed;

	void Start () {
		mousePos = new Vector2();
	}

	void Update () 
	{
		psychonautStyle();
	}

	void psychonautStyle()
	{
		mousePos.x = Input.GetAxis("Mouse X");
		mousePos.y = Input.GetAxis("Mouse Y");

		// rotate vertically
		if (transform.eulerAngles.x + mousePos.y * mouseSensitivity.y < yAxisUpperAngleBound)
			transform.eulerAngles = new Vector3 (yAxisUpperAngleBound, transform.eulerAngles.y, transform.eulerAngles.z);
		else if (transform.eulerAngles.x + mousePos.y * mouseSensitivity.y > yAxisLowerAngleBound)
	 		transform.eulerAngles = new Vector3 (yAxisLowerAngleBound, transform.eulerAngles.y, transform.eulerAngles.z);
		else
			transform.RotateAround (Vector3.zero, transform.right, mousePos.y * mouseSensitivity.y);

		// rotate horizontally
		transform.RotateAround (Vector3.zero, Vector3.up, mousePos.x * mouseSensitivity.x);

		// rotate to make player run in circle
		Vector3 xzSpeed = new Vector3(playerCharacter.rigidbody.velocity.x, 0, playerCharacter.rigidbody.velocity.z);
		float xzDist = Mathf.Sqrt (Mathf.Pow (transform.position.x - playerCharacter.transform.position.x, 2) + Mathf.Pow (transform.position.z - playerCharacter.transform.position.z, 2));
		float rightSpeed = Vector3.Dot (xzSpeed, transform.right);
		if (xzDist == 0)
			xzDist = .0001f;  // prevent divide by zero
		float deltaAngle = rightSpeed / xzDist;
		transform.RotateAround (Vector3.zero, Vector3.up, deltaAngle);

		// set camera based on rotation
		transform.position = playerCharacter.transform.position - transform.forward * maxCameraOffset;
	}
}
