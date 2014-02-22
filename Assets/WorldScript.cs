using UnityEngine;
using System.Collections.Generic;

public class WorldScript : GameBehaviour 
{
	public static PlayerController thePlayer;
	public static List<IResettable> objectsToReset = new List<IResettable>();

	public static float drawDistanceSquared = 210;
	void Awake()
	{
		drawDistanceSquared *= drawDistanceSquared;
	}
	override protected void Start()
	{
		base.Start ();

		thePlayer = GetComponentInChildren<PlayerController>();
		Config.Initialize();
		/*foreach(Transform go in GetComponentsInChildren<Transform>())
		{
			if(go.renderer != null && go.gameObject.GetComponent<DistanceRenderer>() == null)
			{
				go.gameObject.AddComponent<DistanceRenderer>();
			}
		}*/
	}

	public static void reset()
	{
		foreach (IResettable resettable in objectsToReset) 
		{
			if (resettable != null)
				resettable.Reset ();
		}
		GameBehaviour.UnHideAll ();
	}
}