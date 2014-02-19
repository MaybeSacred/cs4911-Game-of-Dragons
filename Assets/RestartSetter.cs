﻿using UnityEngine;
using System.Collections;

public class RestartSetter : MonoBehaviour {
	public Vector3 desiredPosition;
	public Vector3 desiredRotation;
	public int checkPointNumber;
	void Start () {
	
	}
	void OnTriggerEnter(Collider other)
	{
		if(other.tag.Equals("Player"))
		{
			WorldScript.player.SetLastCheckpoint(desiredPosition + transform.position, desiredRotation, checkPointNumber);
		}
	}
}
