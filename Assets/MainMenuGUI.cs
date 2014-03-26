using UnityEngine;
using System.Collections;

/// <summary>
/// Controls the GUI on the main menu. Has buttons
/// to start new game.
/// </summary>
public class MainMenuGUI : GameBehaviour 
{
	
	public GUIStyle currentStyle;

	public GUISkin currentSkin;

	private int screenMargin;

	public Texture2D titleTex;
	
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

		GUI.Label(new Rect(0, 0, 300, 300), titleTex, currentStyle);

		bool newGameButton = GUI.Button ( new Rect(10, 80, 100, 30), "New Game" );
		bool optionsButton = GUI.Button ( new Rect(10, 110, 100, 30), "Options" );
		bool exitButton = GUI.Button ( new Rect(10, 140, 100, 30), "Exit" );

		if (newGameButton) 
			Application.LoadLevel("Scene0");

		if (exitButton)
			Application.Quit ();

		GUI.EndGroup ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
