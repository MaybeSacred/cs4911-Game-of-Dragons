using UnityEngine;
using System.Collections;

public class SnowmanPartController : GameBehaviour 
{
	private float startScale;
	public float fireResistance;
	public float deathShrinkRate;

	override protected void Start()
	{
		base.Start ();

		startScale = transform.localScale.x;
	}
	
	void OnTriggerStay(Collider other)
	{
		switch (other.gameObject.tag) 
		{
		case "FireBreath":
			transform.localScale = new Vector3(
				transform.localScale.x - fireResistance*WorldScript.thePlayer.GetAttackDamage() * Time.deltaTime, 
				transform.localScale.y - fireResistance*WorldScript.thePlayer.GetAttackDamage() * Time.deltaTime,
				transform.localScale.z - fireResistance*WorldScript.thePlayer.GetAttackDamage() * Time.deltaTime
			);
			break;
		}
	}

	public bool isDead()
	{
		return transform.localScale.x < startScale * .5;
	}

	public void DeathShrink()
	{
		transform.localScale = new Vector3(
			transform.localScale.x - deathShrinkRate * Time.deltaTime, 
			transform.localScale.y - deathShrinkRate * Time.deltaTime,
			transform.localScale.z - deathShrinkRate * Time.deltaTime
		);

		rigidbody.constraints = RigidbodyConstraints.None;
		transform.parent = null;
		
		if (transform.localScale.x < 0)
			Destroy(gameObject);
	}

	void Update()
	{

	}
}
