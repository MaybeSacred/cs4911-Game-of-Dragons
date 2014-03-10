using UnityEngine;
using System.Collections;

/// <summary>
/// Destroys the rocks that collide with this instance.
/// </summary>
public class RollingStoneCollector : GameBehaviour 
{

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
