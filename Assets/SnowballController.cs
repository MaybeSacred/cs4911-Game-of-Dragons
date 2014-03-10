using UnityEngine;
using System.Collections;

/// <summary>
/// Controls projectile snowballs that can damage the player.
/// </summary>
public class SnowballController : GameBehaviour 
{
	public bool startMelting;
	public bool isDangerous;
	private float dangerTimer = 0;  // sets isDangerous to false after counting down to 0
	public int damageToPlayer = 1;

	override protected void Start()
	{
		base.Start ();

		startMelting = false;
	}

	void OnCollisionEnter(Collision theCollision)
	{
		switch (theCollision.gameObject.tag) 
		{
		case "Player":
			if (isDangerous)
			{
				isDangerous = false;
				PlayerController pc = (PlayerController)(theCollision.gameObject.GetComponent("PlayerController"));
				pc.HealthChange(-damageToPlayer);
			}
			break;
		}
		if (isDangerous)
			dangerTimer = .25f;
	}

	/// <summary>
	/// Tell the snowball to start its melting sequence.
	/// </summary>
	public void setStartMelting()
	{
		startMelting = true;
	}

	void Update()
	{
		if (dangerTimer > 0) 
		{
			dangerTimer -= Time.deltaTime;
			if (dangerTimer <= 0)
				isDangerous = false;
		}

		if (startMelting) 
		{
			transform.localScale -= new Vector3(transform.localScale.x*.02f, transform.localScale.y*.02f, transform.localScale.z*.02f);
			if (transform.localScale.x < 0)
				Destroy(transform);
		}
	}
}
