using UnityEngine;
using System.Collections.Generic;

public class WorldScript : MonoBehaviour 
{
	public static PlayerController thePlayer;
	public List<IResettable> objectsToReset;
	public static float drawDistanceSquared = 40000;
	void Start()
	{
		thePlayer = GetComponentInChildren<PlayerController>();
		Config.Initialize();
		foreach(Transform go in GetComponentsInChildren<Transform>())
		{
			if(go.renderer != null)
			{
				go.gameObject.AddComponent<DistanceRenderer>();
			}
		}
	}
}