﻿using UnityEngine;
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
	public float additionalCameraOffset;
	public float cameraYZoomOffset;
	public bool enableCollisionZoom = true;
	public float scrollSpeed;

	private Vector3 resetPosition;

	private bool isColliding;

	private Vector3 resetRotation;

	override protected void Start()
	{
		base.Start ();

		yAxisUpperAngleBound += 360;
		mousePos = new Vector2();
		//attemptedCameraOffset = (maxZoomCameraOffset+minZoomCameraOffset)/2;

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
		trueCameraOffset -= Input.GetAxisRaw("Mouse ScrollWheel")*Time.deltaTime*trueZoomSpeed;
		trueCameraOffset = Mathf.Clamp(trueCameraOffset, minZoomCameraOffset, maxZoomCameraOffset);
		if(enableCollisionZoom)
		{
			RaycastHit hit;
			if(Physics.Raycast(WorldScript.thePlayer.transform.localPosition, -transform.forward, out hit, maxZoomCameraOffset, WorldScript.cameraIgnoreLayers))
			{
				if(hit.distance < trueCameraOffset)
				{
					attemptedCameraOffset = Mathf.Lerp(attemptedCameraOffset, -trueCameraOffset+hit.distance-additionalCameraOffset, attemptedZoomSpeed * Time.deltaTime);
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
		}
		currentCameraOffset = Mathf.Lerp(currentCameraOffset, trueCameraOffset + attemptedCameraOffset, Time.deltaTime*zoomSpeed);
		// set camera based on rotation
		transform.position = new Vector3(playerCharacter.transform.position.x - transform.forward.x * currentCameraOffset,
		                                 playerCharacter.transform.position.y - transform.forward.y * currentCameraOffset + (cameraYZoomOffset-currentCameraOffset/maxZoomCameraOffset),
		                                 playerCharacter.transform.position.z - transform.forward.z * currentCameraOffset);
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
	float shakeAmplitude;
	public float maxCameraShakeAmplitude;
	float shakeTimer;
	public float minimumCameraShakeAmplitude;
	public float shakeAmplitudeDecayRate;
	public float shakeRate;
	public void ActivateCameraShake(float amplitude)
	{
		shakeAmplitude = Mathf.Clamp(amplitude, 0, maxCameraShakeAmplitude);
		shakeTimer = 0.001f;
	}
	private void ShakeCamera()
	{
		if(shakeTimer > 0)
		{
			shakeAmplitude = Mathf.Lerp(shakeAmplitude, 0, shakeAmplitudeDecayRate * Time.deltaTime);
			transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, shakeAmplitude * Mathf.Sin(shakeRate * Time.timeSinceLevelLoad));
			if(shakeAmplitude < minimumCameraShakeAmplitude)
			{
				shakeTimer = 0;
				transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);
			}
			shakeTimer += Time.deltaTime;
		}
	}
}
