using UnityEngine;
using System.Collections;

public class CoinController : GameBehaviour {

	public float rotationSpeed;

	override protected void Start()
	{
		base.Start ();
		
	}
	
	void Update () 
	{
		transform.rotation *= new Quaternion(0, Mathf.Sin(rotationSpeed*Time.deltaTime), 0, Mathf.Cos(rotationSpeed*Time.deltaTime));
	}
}
