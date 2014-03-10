using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// An extension of MonoBehaviour. All scripts should
/// extend this. This adds the ability to hide and 
/// unhide objects without destroying them.
/// </summary>
public class GameBehaviour : MonoBehaviour
{
	/// <summary>
	/// A list of all instances that are currently hidden
	/// </summary>
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
	
	/// <returns><c>true</c>, if hidden, <c>false</c> otherwise.</returns>
	public bool isHidden()
	{
		return hidden;
	}

	/// <summary>
	/// Hide this instance.
	/// </summary>
	public void Hide()
	{
		hidden = true;
		gameObject.SetActive (false);
		hiddenObjects.Add (this);
	}

	/// <summary>
	/// Unhides this instance
	/// </summary>
	public void UnHide()
	{
		hidden = false;
		gameObject.SetActive (true);
		hiddenObjects.Remove (this);
	}

	/// <summary>
	/// Unhides all instances
	/// </summary>
	public static void UnHideAll()
	{
		foreach (GameBehaviour hidden in hiddenObjects)
			hidden.gameObject.SetActive (true);

		hiddenObjects.Clear ();
	}
}