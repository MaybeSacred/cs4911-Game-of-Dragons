using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Controls the switch that opens the castle gate.
/// </summary>
public class CrystalBallBehaviour : MonoBehaviour 
{
	public Transform gate;
	private bool movingGate;
	void Start () {
		
	}
	
	void Update () {
		if(movingGate)
		{
			
		}
	}
	void OnTriggerEnter(Collider other)
	{
		if(other.tag.Equals("Player"))
		{
			movingGate = true;
			WorldScript.theCamera.ActivateCameraShake(3);
		}
	}
}
