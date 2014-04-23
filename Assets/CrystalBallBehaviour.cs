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

	/// <summary>
	/// Opens the castle gate when touched and when player has enough gems
	/// </summary>
	/// <param name="other">Object colliding with crystal ball</param>
	void OnTriggerEnter(Collider other)
	{
		PlayerController player = (PlayerController)GameObject.Find ("Player").GetComponent("PlayerController");

		if(!movingGate && player.gems >= WorldScript.getTotalGems() * .7) // allow player to miss 30% of gems
		{
			if(other.tag.Equals("FireBreath") || other.tag.Equals ("Player"))
			{
				gate.Activate();
				movingGate = true;
				WorldScript.theCamera.ActivateCameraShake(cameraShakeAmplitude);
			}
		}
	}
}
