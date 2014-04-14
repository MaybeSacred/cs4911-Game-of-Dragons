using UnityEngine;
using System.Collections;

/// <summary>
/// Controls the GUI on the main menu. Has buttons
/// to start new game.
/// </summary>
public class EndGUI : GameBehaviour 
{
	
	public GUIStyle currentStyle;
	
	public GUISkin currentSkin;
	
	private int screenMargin;
	
	//public Texture2D titleTex;
	
	// Use this for initialization
	override protected void Start () 
	{
		base.Start ();
		
		screenMargin = 25;
	}
	
	void OnGUI()
	{
		GUI.skin = currentSkin;
		
		GUI.BeginGroup(new Rect(screenMargin, screenMargin, Screen.width-screenMargin*2, Screen.height-screenMargin*2), currentStyle);
		
		GUI.Label(new Rect(0, 0, 300, 300), "Game completed", currentStyle);

		GUI.Label(new Rect(0, 80, 300, 300), "Time Taken: " + (PlayerPrefs.GetFloat("totalTime")/60) + " minutes", currentStyle);
		GUI.Label(new Rect(0, 120, 300, 300), "Total Coins: " + PlayerPrefs.GetInt("totalCoins"), currentStyle);
		GUI.Label(new Rect(0, 160, 300, 300), "Total Gems: " + PlayerPrefs.GetInt("totalGems"), currentStyle);
		
		GUI.EndGroup ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
