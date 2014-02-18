using UnityEngine;
using System.Collections;

public class WorldScript : MonoBehaviour 
{
	public static PlayerController thePlayer;

	public ArrayList<IResettable> objectsToReset;

	void Start()
	{
		thePlayer = GetComponentInChildren<PlayerController>();
		Config.init();
	}
}