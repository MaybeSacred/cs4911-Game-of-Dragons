using UnityEngine;
using System.Collections;

/// <summary>
/// Controls the position and direction of the player's camera. Player
/// can control camera using mouse movements and scroll wheel.
/// </summary>
public class CameraScript : GameBehaviour, IResettable
{

	public PlayerController playerCharacter;

	public Vector2 mousePos;
	public Vector2 mouseSensitivity;
	
	public float yAxisUpperAngleBound, yAxisLowerAngleBound;

	public float attemptedZoomSpeed;
	public float zoomSpeed;
	public float trueZoomSpeed;
	private float currentCameraOffset, attemptedCameraOffset, trueCameraOffset;
	public float minZoomCameraOffset, maxZoomCameraOffset;
	public float additionalCameraOffset;
	public float cameraYZoomOffset;
	public bool enableCollisionZoom = true;

	private float shakeAmplitude;
	private float shakeTimer;
	public float maxCameraShakeAmplitude;
	public float minimumCameraShakeAmplitude;
	public float shakeAmplitudeDecayRate;
	public float shakeRate;

	private Vector3 resetPosition;
	private Vector3 resetRotation;

	override protected void Start()
	{
		base.Start ();

		yAxisUpperAngleBound += 360;
		mousePos = new Vector2();

		SaveState ();
	}

	void Update () 
	{
		Screen.lockCursor = true;
		psychonautStyle();
	}

	/// <summary>
	/// Use a control scheme similar to the PC platformer, Psychonauts
	/// </summary>
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

		if(shakeTimer > 0)
			ShakeCamera();

		currentCameraOffset = Mathf.Lerp(currentCameraOffset, trueCameraOffset + attemptedCameraOffset, Time.deltaTime*zoomSpeed);

		// set camera based on rotation
		transform.position = new Vector3(playerCharacter.transform.position.x - transform.forward.x * currentCameraOffset,
		                                 playerCharacter.transform.position.y - transform.forward.y * currentCameraOffset + (cameraYZoomOffset-currentCameraOffset/maxZoomCameraOffset),
		                                 playerCharacter.transform.position.z - transform.forward.z * currentCameraOffset);
	}

	/// <seealso cref="IResettable"/>
	public void SaveState()
	{
		resetPosition = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
		resetRotation = new Vector3 (transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
	}

	/// <seealso cref="IResettable"/>
	public void Reset()
	{
		transform.position = resetPosition;
		transform.eulerAngles = resetRotation;
	}

	/// <summary>
	/// Make camera shake with sin function with given amplitude.
	/// </summary>
	/// <param name="amplitude">Amplitude.</param>
	public void ActivateCameraShake(float amplitude)
	{
		shakeAmplitude = Mathf.Clamp(amplitude, 0, maxCameraShakeAmplitude);
		shakeTimer = 0.001f;
	}

	/// <summary>
	/// Shakes the camera.
	/// </summary>
	private void ShakeCamera()
	{
		shakeAmplitude = Mathf.Lerp(shakeAmplitude, 0, shakeAmplitudeDecayRate * Time.deltaTime);
		transform.localEulerAngles = new Vector3(transform.localEulerAngles.x + shakeAmplitude*(Random.value-.5f), transform.localEulerAngles.y, shakeAmplitude * Mathf.Sin(shakeRate * Time.timeSinceLevelLoad));
		shakeTimer += Time.deltaTime;
		if(shakeAmplitude < minimumCameraShakeAmplitude)
		{
			shakeTimer = 0;
			transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);
		}
	}
}
