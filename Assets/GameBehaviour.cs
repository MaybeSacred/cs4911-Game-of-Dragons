using UnityEngine;
using System.Collections.Generic;

public class GameBehaviour : MonoBehaviour
{
	public static List<GameBehaviour> hiddenObjects = new List<GameBehaviour>();

	private bool hidden;

	protected virtual void Start()
	{
		hidden = false;

		IResettable resettable = this as IResettable;
		if (resettable != null) 
		{
			WorldScript.objectsToReset.Add(resettable);
		}
	}

	public bool isHidden()
	{
		return hidden;
	}

	public void Hide()
	{
		hidden = true;
		gameObject.SetActive (false);
		hiddenObjects.Add (this);
	}

	public void UnHide()
	{
		hidden = false;
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