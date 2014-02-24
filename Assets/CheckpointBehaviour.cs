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
	
	// Update is called once per frame
	void Update () 
	{
		letterC.Rotate(0, letterCRotateSpeed, 0);
	}
}
