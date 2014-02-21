using UnityEngine;
using System.Collections.Generic;

public class GameBehaviour : MonoBehaviour
{
	public static List<GameBehaviour> hiddenObjects = new List<GameBehaviour>();

	protected virtual void Start()
	{
		IResettable resettable = this as IResettable;
		if (resettable != null) 
		{
			WorldScript world = (WorldScript)(GameObject.Find("World").GetComponent("WorldScript"));
			world.objectsToReset.Add(resettable);
		}
	}

	public void Hide()
	{
		gameObject.SetActive (false);
		hiddenObjects.Add (this);
	}

	public void UnHide()
	{
		gameObject.SetActive (true);
		hiddenObjects.Remove (this);
	}

	public static void UnHideAll()
	{
		foreach (GameBehaviour hidden in hiddenObjects)
			hidden.gameObject.SetActive (true);

		hiddenObjects.Clear ();
	}
}