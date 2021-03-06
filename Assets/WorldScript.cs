using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// An interface to help control sets of objects within the world.
/// </summary>
public class WorldScript : GameBehaviour 
{
	public static PlayerController thePlayer;
	public static CameraScript theCamera;
	public static List<IResettable> objectsToReset = new List<IResettable>();
	public static int cameraIgnoreLayers;

	public static float totalTime = 0;

	void Awake()
	{
		cameraIgnoreLayers = (1<<LayerMask.NameToLayer("Default") | 1<<LayerMask.NameToLayer("Icy"));

		objectsToReset.Clear ();
	}

	override protected void Start()
	{
		base.Start ();
		theCamera = GetComponentInChildren<CameraScript>();
		thePlayer = GetComponentInChildren<PlayerController>();
		Config.Initialize();

		totalTime = 0;
	}

	void Update()
	{
		totalTime += Time.deltaTime;
	}

	/// <summary>
	/// Returns true if given resettable is hidden.
	/// </summary>
	/// <returns><c>true</c>, if resettable should be removed, <c>false</c> otherwise.</returns>
	/// <param name="resettable">Resettable object</param>
	public static bool removeHiddenPredicate(IResettable resettable)
	{
		GameBehaviour behaviour = resettable as GameBehaviour;
		if (behaviour != null) 
		{
			return behaviour.isHidden ();
		}

		return false;
	}

	/// <summary>
	/// Gets the number of large gems plus the number of large gems that can be made with small gems.
	/// </summary>
	/// <returns>The number of large gems plus the number of large gems that can be made with small gems</returns>
	public static int getTotalGems()
	{
		return CoinController.totalLargeGems + (int)(CoinController.totalSmallGems / PlayerController.FRAGMENTS_PER_GEM);
	}

	/// <summary>
	/// Removes hidden objects and saves the state of all remaining objects.
	/// </summary>
	public static void save()
	{
		objectsToReset.RemoveAll (removeHiddenPredicate);
		foreach (IResettable resettable in objectsToReset)
			if (resettable != null)
				resettable.SaveState ();
	}

	/// <summary>
	/// Unhide all hidden objects and reset them to their saved states.
	/// </summary>
	public static void reset()
	{
		foreach (IResettable resettable in objectsToReset) 
		{
			if (resettable != null)
				resettable.Reset ();
		}
		GameBehaviour.UnHideAll ();
	}

	/// <summary>
	/// Ends the game and opens the end game screen.
	/// </summary>
	public static void EndGame()
	{
		PlayerController player = (PlayerController)GameObject.Find ("Player").GetComponent("PlayerController");

		PlayerPrefs.SetFloat ("totalTime", totalTime);
		PlayerPrefs.SetInt ("totalCoins", player.coins);
		PlayerPrefs.SetInt ("totalGems", player.gems);
		PlayerPrefs.SetInt ("totalSmallGems", player.smallGems);

		Application.LoadLevel("Scene0-end");
	}
}