using UnityEngine;
using System.Collections;

/// <summary>
/// Tells the world to save the state of all Resettable objects
/// when the player begins colliding with the checkpoint.
/// </summary>
public class CheckpointBehaviour : GameBehaviour 
{

	public Transform letterC;
	public float letterCRotateSpeed;
	public Material savedSwitchMaterial;
	private Material unSavedSwitchMaterial;

	// Use this for initialization
	protected override void Start () 
	{
		base.Start ();
		unSavedSwitchMaterial = letterC.renderer.material;
	}

	void OnTriggerEnter(Collider other)
	{
		switch (other.gameObject.tag) 
		{
		case "Player":
			WorldScript.save();
			letterC.renderer.material = savedSwitchMaterial;
			break;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!isHidden())
			letterC.Rotate(0, letterCRotateSpeed*Time.deltaTime, 0);
	}
}
