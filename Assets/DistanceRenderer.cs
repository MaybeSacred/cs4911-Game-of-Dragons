using UnityEngine;
using System.Collections.Generic;

public class DistanceRenderer : GameBehaviour 
{

	override protected void Start()
	{
		base.Start ();
	
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
