using UnityEngine;
using System.Collections;

public class GameGUI : MonoBehaviour {
	public GUIStyle currentStyle;
	public Texture2D healthTex;
	public Texture2D fullGemTex;
	public Texture2D brokenGemTex;
	private Rect gemDisplayRectangle;
	// Use this for initialization
	void Start () {
		gemDisplayRectangle = new Rect(Screen.width - fullGemTex.width*2-brokenGemTex.width*2, 0, fullGemTex.width*2+brokenGemTex.width*2, fullGemTex.height);
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
		GUI.BeginGroup(gemDisplayRectangle, currentStyle);
		GUI.BeginGroup(new Rect(0, 0, gemDisplayRectangle.width/2, gemDisplayRectangle.height));
		GUI.Label(new Rect(0, 0, brokenGemTex.width, brokenGemTex.height), brokenGemTex, currentStyle);
		GUI.Label(new Rect(brokenGemTex.width, 0, brokenGemTex.width, brokenGemTex.height), WorldScript.thePlayer.smallGems.ToString(), currentStyle);
		GUI.EndGroup();
		GUI.BeginGroup(new Rect(gemDisplayRectangle.width/2, 0, gemDisplayRectangle.width/2, gemDisplayRectangle.height));
		GUI.Label(new Rect(0, 0, fullGemTex.width, fullGemTex.height), fullGemTex, currentStyle);
		GUI.Label(new Rect(fullGemTex.width, 0, fullGemTex.width, fullGemTex.height), WorldScript.thePlayer.gems.ToString(), currentStyle);
		GUI.EndGroup();
		GUI.EndGroup();
	}
	void Update () {
	
	}
}
