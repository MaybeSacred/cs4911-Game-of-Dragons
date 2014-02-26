using UnityEngine;
using System.Collections;

public class CheckpointBehaviour : GameBehaviour 
{

	public Transform letterC;
	public float letterCRotateSpeed;

	// Use this for initialization
	void Start () 
	{
		base.Start ();
	}

	void OnTriggerEnter(Collider other)
	{
		switch (other.gameObject.tag) 
		{
		case "Player":
			WorldScript.save();
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
