using UnityEngine;
using System.Collections;

public class RollingStoneCollector : GameBehaviour {

	override protected void Start()
	{
		base.Start ();
	
	}
	
	void Update () {
	
	}
	void OnTriggerEnter(Collider other)
	{
		if(other.tag.Equals("RollingRock"))
		{
			Destroy(other.gameObject);
		}
	}
}
