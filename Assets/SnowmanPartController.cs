using UnityEngine;
using System.Collections;

/// <summary>
/// Controls an individual body part of the snowman
/// </summary>
public class SnowmanPartController : GameBehaviour, IResettable
{
	private float startScale;
	public float fireResistance;
	public float deathShrinkRate;

	private Transform resetParent;
	private Vector3 resetPosition;
	private Vector3 resetScale;
	private Vector3 resetRotation;

	override protected void Start()
	{
		base.Start ();

		startScale = transform.localScale.x;

		SaveState ();
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

	/// <summary>
	/// Shrinks the body part and hides it once it is small enough.
	/// </summary>
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
		{
			Hide ();
			//Destroy (gameObject);
		}
	}

	void Update()
	{

	}

	/// <seealso cref="IResettable"/>
	public void SaveState()
	{
		resetParent = transform.parent;
		resetPosition = transform.position;
		resetRotation = transform.localEulerAngles;
		resetScale = transform.localScale;
	}

	/// <seealso cref="IResettable"/>
	public void Reset()
	{
		transform.parent = resetParent;
		transform.position = resetPosition;
		transform.localEulerAngles = resetRotation;
		transform.localScale = resetScale;
		rigidbody.constraints = RigidbodyConstraints.FreezePositionX | 
								RigidbodyConstraints.FreezePositionZ | 
								RigidbodyConstraints.FreezeRotationX |
								RigidbodyConstraints.FreezeRotationY |
								RigidbodyConstraints.FreezeRotationZ; 
	}
}
