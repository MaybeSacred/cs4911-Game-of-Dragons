using UnityEngine;
using System.Collections;

public class CheckpointBehaviour : GameBehaviour 
{

	public Transform letterC;
	private float letterCRotateSpeed;

	// Use this for initialization
	void Start () 
	{
		base.Start ();

		letterCRotateSpeed = 5;
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
		if (letterC != null)
			letterC.Rotate(0, letterCRotateSpeed, 0);
	}
}
