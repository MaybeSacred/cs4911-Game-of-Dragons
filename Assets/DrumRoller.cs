using UnityEngine;
using System.Collections;

public class DrumRoller : MonoBehaviour {
	public bool isOscillating;
	public float rotationSpeedDegrees;
	private float oscillationTimer;
	public float totalRotationDegrees;
	public float deadZoneAngle;
	void Start () {
		Debug.Log("hi");
		Debug.Log("hi");
		Debug.Log("hi");
		Debug.Log("hi");
		Debug.Log("hi");
	}
	
	void Update () {
		if(isOscillating)
		{
			
		}
		else
		{
			transform.rotation *= new Quaternion(0, 0, Mathf.Sin(rotationSpeedDegrees*Time.deltaTime), Mathf.Cos(rotationSpeedDegrees*Time.deltaTime));
		}
	}
}
