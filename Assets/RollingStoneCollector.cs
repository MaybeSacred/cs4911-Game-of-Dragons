using UnityEngine;
using System.Collections;

public class RollingStoneCollector : MonoBehaviour {

	void Start () {
	
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
