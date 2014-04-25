using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Controller for the portal that ends the game when
/// the player collides with it.
/// </summary>
public class PortalBehavior : MonoBehaviour 
{
	private Material portalMat;
	public float xSpeed, ySpeed;

	void Start () 
	{
		portalMat = renderer.material;
	}
	
	void Update () 
	{
		portalMat.mainTextureOffset += new Vector2(xSpeed * Time.deltaTime, ySpeed * Time.deltaTime);
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag.Equals("Player"))
		{
			WorldScript.EndGame();
		}
	}
}
