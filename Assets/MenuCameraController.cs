using UnityEngine;
using System.Collections;

/// <summary>
/// Controls the camera that moves around on the main menu.
/// </summary>
public class MenuCameraController : GameBehaviour 
{

	public Transform rotationCenter;
	public float rotationRadius;
	public float rotationSpeed;

	private float angleDown;
	private float angleSide;

	// Use this for initialization
	override protected void Start () 
	{
		base.Start ();

		angleDown = 25;
		angleSide = 0;
	}

	// Update is called once per frame
	void Update () 
	{
		transform.eulerAngles = new Vector3 (angleDown, angleSide, 0);

		transform.position = new Vector3(rotationCenter.position.x - transform.forward.x * rotationRadius,
		                                 rotationCenter.position.y - transform.forward.y * rotationRadius,
		                                 rotationCenter.position.z - transform.forward.z * rotationRadius);

		angleSide += rotationSpeed * Time.deltaTime;
	}
}
