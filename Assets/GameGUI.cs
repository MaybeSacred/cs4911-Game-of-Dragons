using UnityEngine;
using System.Collections;

/// <summary>
/// Controls the in-game GUI. Shows health, fire, and gem counts.
/// </summary>
public class GameGUI : GameBehaviour 
{
	public GUIStyle currentStyle;
	public Texture2D healthTex;
	public Texture2D emptyHealthTex;
	public Texture2D fullGemTex;
	public Texture2D brokenGemTex;
	public Texture2D hpBarBackground;
	public Texture2D transHPBarBackground;
	private Rect gemDisplayRectangle;
	public float maxHPBarWidth;
	
	override protected void Start()
	{
		base.Start ();

		gemDisplayRectangle = new Rect(Screen.width - fullGemTex.width*2-brokenGemTex.width*2 - 20, 0, fullGemTex.width*2+brokenGemTex.width*2, fullGemTex.height);
	}

	void OnGUI()
	{
		GUI.BeginGroup(new Rect(0, 0, maxHPBarWidth, hpBarBackground.height), transHPBarBackground, currentStyle);
		GUI.BeginGroup(new Rect(0, 0, maxHPBarWidth*WorldScript.thePlayer.GetFlameScale(), hpBarBackground.height));
		GUI.Label(new Rect(0, 0, maxHPBarWidth, hpBarBackground.height), hpBarBackground, currentStyle);
		GUI.EndGroup();
		for(int i = 0; i < WorldScript.thePlayer.maxHealth; i++)
		{
			if(i < WorldScript.thePlayer.health)
			{
				GUI.Label(new Rect(((float)i*maxHPBarWidth)/WorldScript.thePlayer.maxHealth, 0, maxHPBarWidth/WorldScript.thePlayer.maxHealth, 32), healthTex, currentStyle);
			}
			else
			{
				GUI.Label(new Rect(((float)i*maxHPBarWidth)/WorldScript.thePlayer.maxHealth, 0, maxHPBarWidth/WorldScript.thePlayer.maxHealth, 32), emptyHealthTex, currentStyle);
			}
		}
		GUI.EndGroup();
		GUI.BeginGroup(gemDisplayRectangle, currentStyle);
		GUI.BeginGroup(new Rect(0, 0, gemDisplayRectangle.width/2, gemDisplayRectangle.height));
		GUI.Label(new Rect(0, 0, brokenGemTex.width, brokenGemTex.height), brokenGemTex, currentStyle);
		GUI.Label(new Rect(brokenGemTex.width, 0, brokenGemTex.width, brokenGemTex.height), WorldScript.thePlayer.smallGems.ToString() + "/4", currentStyle);
		GUI.EndGroup();
		GUI.BeginGroup(new Rect(gemDisplayRectangle.width/2, 0, gemDisplayRectangle.width/2, gemDisplayRectangle.height));
		GUI.Label(new Rect(0, 0, fullGemTex.width, fullGemTex.height), fullGemTex, currentStyle);
		GUI.Label(new Rect(fullGemTex.width, 0, fullGemTex.width, fullGemTex.height), WorldScript.thePlayer.gems.ToString() + "/" + (int)(WorldScript.getTotalGems()*.7f), currentStyle);
		GUI.EndGroup();
		GUI.EndGroup();
	}

	void Update () 
	{
	
	}
}
