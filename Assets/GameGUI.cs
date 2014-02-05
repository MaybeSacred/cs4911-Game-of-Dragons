using UnityEngine;
using System.Collections;

public class GameGUI : MonoBehaviour {
	public GUIStyle currentStyle;
	public Texture2D healthTex;
	// Use this for initialization
	void Start () {
	
	}
	void OnGUI()
	{
		GUI.BeginGroup(new Rect(0, 0, 250, 32));
		GUI.Label(new Rect(0, 0, 250, 32), GUIContent.none, currentStyle);
		for(int i = 0; i < WorldScript.thePlayer.health; i++)
		{
			GUI.Label(new Rect(((float)i*250)/WorldScript.thePlayer.maxHealth, 0, 25, 32), healthTex, currentStyle);
		}
		GUI.EndGroup();
	}
	void Update () {
	
	}
}
