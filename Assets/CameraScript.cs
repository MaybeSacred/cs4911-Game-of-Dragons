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
		/*Debug.Log("hi");Debug.Log("hi");
		Debug.Log("hi");Debug.Log("hi");Debug.Log("hi");Debug.Log("hi");
		Debug.Log("hi");Debug.Log("hi");Debug.Log("hi");Debug.Log("hi");
		Debug.Log("hi");Debug.Log("hi");
		Debug.Log("hi");Debug.Log("hi");*/
	}

	void Update () 
	{
		switch (1) 
		{
		case 1:
			psychonautStyle();
			break;
		case 2:
			oldCamera();
			break;
		}
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
		float xzDist = Mathf.Sqrt (Mathf.Pow (transform.position.x - playerCharacter.transform.position.x, 2) + Mathf.Pow (transform.position.z - playerCharacter.transform.position.z, 2));
		Vector3 xzSpeed = new Vector3(playerCharacter.rigidbody.velocity.x, 0, playerCharacter.rigidbody.velocity.z);
		float rightSpeed = Vector3.Dot (xzSpeed, transform.right);
		if (xzDist == 0)
			xzDist = .0001f;  // prevent divide by zero
		float deltaAngle = rightSpeed / xzDist;
		transform.RotateAround (Vector3.zero, Vector3.up, deltaAngle);

		// set camera based on rotation
		transform.position = playerCharacter.transform.position - transform.forward * maxCameraOffset;
	}

	void oldCamera () {
		/*
		mousePos.x = Input.GetAxis("Mouse X");
		mousePos.y = Input.GetAxis("Mouse Y");
		Vector3 temp = transform.eulerAngles;
		if(temp.x > 110)
			temp.x -=360;
		if(temp.x - mousePos.y <= yAxisUpperAngleBound)
		{
			transform.eulerAngles = new Vector3(yAxisUpperAngleBound, transform.eulerAngles.y + mousePos.x*mouseSensitivity.x, 0);
		}
		else if(temp.x - mousePos.y >= yAxisLowerAngleBound)
		{
			transform.eulerAngles = new Vector3(yAxisLowerAngleBound, transform.eulerAngles.y + mousePos.x*mouseSensitivity.x, 0);
		}
		else
		{
			transform.eulerAngles += new Vector3(mousePos.y*mouseSensitivity.y, mousePos.x*mouseSensitivity.x, 0);
		}
		float scrolling = Input.GetAxis("Mouse ScrollWheel");
		cameraOffset -= scrollSpeed*scrolling*cameraOffset*Time.deltaTime;
		if(cameraOffset.magnitude <= minCameraOffset)
			cameraOffset = cameraOffset.normalized*minCameraOffset;
		if(cameraOffset.magnitude >= maxCameraOffset)
			cameraOffset = cameraOffset.normalized*maxCameraOffset;
			*/
		if(Input.GetAxisRaw("Vertical") < 0)
		{
			//cameraOffset = Vector3.RotateTowards(cameraOffset, -playerCharacter.transform.forward*minCameraOffset, rotationSpeed*Time.deltaTime, 1);
		}
		else if(Input.GetAxisRaw("Vertical") > 0)
		{
			//cameraOffset = Vector3.RotateTowards(cameraOffset, playerCharacter.transform.forward*minCameraOffset, rotationSpeed*Time.deltaTime, 1);
		}
		if(Input.GetAxisRaw("Horizontal") != 0)
		{
			cameraOffset = Vector3.RotateTowards(cameraOffset, playerGraphics.forward*minCameraOffset, movingRotationSpeed*Time.deltaTime, 1);
		}
		else
		{
			cameraOffset = Vector3.RotateTowards(cameraOffset, playerGraphics.forward*minCameraOffset, stationaryRotationSpeed*Time.deltaTime, 1);
		}
		transform.forward = cameraOffset.normalized;
		transform.localPosition = new Vector3(playerCharacter.transform.localPosition.x - cameraOffset.x, 
		                                      playerCharacter.transform.localPosition.y + 5, 
		                                      playerCharacter.transform.localPosition.z - cameraOffset.z);
		/*transform.localPosition = new Vector3(playerCharacters[currentCharacter].localPosition.x - cameraOffset.x*Mathf.Sin(Mathf.Deg2Rad*transform.eulerAngles.y), 
											  playerCharacters[currentCharacter].localPosition.y + cameraOffset.y, 
											  playerCharacters[currentCharacter].localPosition.z - cameraOffset.x*Mathf.Cos(Mathf.Deg2Rad*transform.eulerAngles.y));*/
	}
}
