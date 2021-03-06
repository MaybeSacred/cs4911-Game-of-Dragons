﻿using UnityEngine;
using System.Collections;

/// <summary>
/// Controls a set rotation on object this is applied to.
/// </summary>
public class DrumRoller : GameBehaviour 
{
	public bool isOscillating;
	public float rotationSpeedDegrees;
	private float oscillationTimer;
	public float totalRotationDegrees;
	public float deadZoneAngle;

	override protected void Start()
	{
		base.Start ();
	}
	
	void Update () 
	{
		if(isOscillating)
		{
			
		}
		else
		{
			transform.rotation *= new Quaternion(0, 0, Mathf.Sin(rotationSpeedDegrees*Time.deltaTime), Mathf.Cos(rotationSpeedDegrees*Time.deltaTime));
		}
	}
}
