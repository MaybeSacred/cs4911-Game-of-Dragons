using UnityEngine;
using System.Collections.Generic;

public class PortalBehavior : MonoBehaviour {
	private Material portalMat;
	public float xSpeed, ySpeed;
	void Start () {
		portalMat = renderer.material;
	}
	
	void Update () {
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
