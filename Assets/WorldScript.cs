using UnityEngine;
using System.Collections.Generic;

public class WorldScript : GameBehaviour 
{
	public static PlayerController thePlayer;
	public static List<IResettable> objectsToReset = new List<IResettable>();
	public static int cameraIgnoreLayers;
	void Awake()
	{
		Debug.Log(LayerMask.NameToLayer("Default"));
		Debug.Log(LayerMask.NameToLayer("Icy"));
		cameraIgnoreLayers = (1<<LayerMask.NameToLayer("Default") | 1<<LayerMask.NameToLayer("Icy"));
	}
	override protected void Start()
	{
		base.Start ();

		thePlayer = GetComponentInChildren<PlayerController>();
		Config.Initialize();
	}

	public static bool removeHiddenPredicate(IResettable resettable)
	{
		GameBehaviour behaviour = resettable as GameBehaviour;
		if (behaviour != null && behaviour.isHidden()) 
		{
			return true;
		}

		return false;
	}

	public static void save()
	{
		foreach (IResettable resettable in objectsToReset)
		{
			if (resettable != null)
			{
				GameBehaviour behaviour = resettable as GameBehaviour;
				if (behaviour != null)
					GameBehaviour.hiddenObjects.Remove(behaviour);
			}
		}

		objectsToReset.RemoveAll (removeHiddenPredicate);

		foreach (IResettable resettable in objectsToReset)
		{
			if (resettable != null)
			{
				resettable.SaveState ();
			}
		}
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