using UnityEngine;
using System.Collections;

public class WorldScript : MonoBehaviour 
{
	public static PlayerController thePlayer;
	void Start()
	{
		thePlayer = GetComponentInChildren<PlayerController>();
		Config.init();
	}
}