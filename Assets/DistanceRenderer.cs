using UnityEngine;
using System.Collections.Generic;

public class DistanceRenderer : MonoBehaviour {

	void Start () {
	
	}
	
	void Update () {
		if((transform.position - WorldScript.thePlayer.transform.position).sqrMagnitude < WorldScript.drawDistanceSquared)
		{
			renderer.enabled = true;
		}
		else
		{
			renderer.enabled = false;
		}
	}
}
