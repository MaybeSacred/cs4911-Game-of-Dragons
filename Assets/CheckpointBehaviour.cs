using UnityEngine;
using System.Collections;

public class CheckpointBehaviour : GameBehaviour 
{

	public Transform letterC;
	public float letterCRotateSpeed;
	public Material savedSwitchMaterial;
	private Material unSavedSwitchMaterial;
	// Use this for initialization
	void Start () 
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
