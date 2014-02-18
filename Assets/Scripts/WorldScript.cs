using UnityEngine;
using System.Collections.Generic;

public class WorldScript : MonoBehaviour 
{
	public static PlayerController thePlayer;

	public List<IResettable> objectsToReset;

	void Start()
	{
		thePlayer = GetComponentInChildren<PlayerController>();
		Config.init();
	}
}