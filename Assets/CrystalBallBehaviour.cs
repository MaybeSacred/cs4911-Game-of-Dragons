using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Controls the switch that opens the castle gate.
/// </summary>
public class CrystalBallBehaviour : MonoBehaviour 
{
	public GateBehaviour gate;
	private bool movingGate;
	public float cameraShakeAmplitude;
	void Start () {
		
	}
	
	void Update () {
	
	}
	void OnTriggerEnter(Collider other)
	{
		if(!movingGate)
		{
			if(other.tag.Equals("Player"))
			{
				gate.Activate();
				movingGate = true;
				WorldScript.theCamera.ActivateCameraShake(cameraShakeAmplitude);
			}
		}
	}
}
