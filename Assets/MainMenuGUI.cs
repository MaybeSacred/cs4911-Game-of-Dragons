using UnityEngine;
using System.Collections;

/// <summary>
/// Controls the GUI on the main menu. Has buttons
/// to start new game.
/// </summary>
public class MainMenuGUI : GameBehaviour 
{
	
	public GUIStyle currentStyle;
	private int screenMargin;
	
	// Use this for initialization
	override protected void Start () 
	{
		base.Start ();
		
		screenMargin = 25;
	}
	
	void OnGUI()
	{
		GUI.BeginGroup(new Rect(screenMargin, screenMargin, Screen.width-screenMargin*2, Screen.height-screenMargin*2), currentStyle);

		GUI.Label(new Rect(0, 0, 300, 300), "Game of Dragons", currentStyle);

		bool newGameButton = GUI.Button ( new Rect(0, 100, 100, 40), "New Game" );
		bool optionsButton = GUI.Button ( new Rect(0, 140, 100, 40), "Options" );
		bool exitButton = GUI.Button ( new Rect(0, 180, 100, 40), "Exit" );

		if (newGameButton) 
		{
			Application.LoadLevel("Scene0");
		}

		GUI.EndGroup ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
