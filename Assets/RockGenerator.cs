﻿using UnityEngine;
using System.Collections;

/// <summary>
/// Continually creates rocks that fall in the level.
/// </summary>
public class RockGenerator : GameBehaviour 
{
	public float timeBetweenRocks;
	public Transform[] rocks;
	private float emissionTimer;
	public float rockScalePermutePercent;
	public float density;

	override protected void Start()
	{
		base.Start ();
	
	}
	
	// Update is called once per frame
	void Update()
	{
		if(emissionTimer >= timeBetweenRocks)
		{
			Transform temp = (Transform)Instantiate(rocks[Mathf.FloorToInt(Random.Range(0, rocks.Length))], new Vector3(transform.position.x+Random.Range(-transform.localScale.x/2, transform.localScale.x/2),
			                                                                                transform.position.y+Random.Range(-transform.localScale.y/2, transform.localScale.y/2),
			                                                                                transform.position.z+Random.Range(-transform.localScale.z/2, transform.localScale.z/2)), Quaternion.identity);
			PermuteRockAttributes(temp);
			emissionTimer -= timeBetweenRocks;
		}
		emissionTimer += Time.deltaTime;
	}

	private void PermuteRockAttributes(Transform indy)
	{
		indy.localScale = indy.localScale*Random.Range(rockScalePermutePercent, 2-rockScalePermutePercent);
		indy.rigidbody.mass = indy.localScale.x*indy.localScale.x*indy.localScale.x*density;
	}
}
