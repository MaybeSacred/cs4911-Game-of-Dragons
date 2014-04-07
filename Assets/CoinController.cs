using UnityEngine;
using System.Collections;

/// <summary>
/// Controls the spinning and collection behavior of all token
/// objects. This includes things that are not coins, such as
/// gems.
/// </summary>
public class CoinController : GameBehaviour, IResettable 
{

	public float rotationSpeed;

	private Vector3 originalPosition;

	private bool bouncing = false;
	private float vy = 0;
	private float ay = 0;


	override protected void Start()
	{
		base.Start ();
	}

	void OnTriggerEnter(Collider other)
	{
		switch (other.gameObject.tag) 
		{
		case "Player":
			PlayerController player = WorldScript.thePlayer;

			switch (gameObject.tag)
			{
			case "Gem":
				Hide ();
				player.incrementGems();
				player.HealthChange(player.maxHealth);
				break;
				
			case "SmallGem":
				Hide ();
				player.incrementSmallGems();
				player.HealthChange(2);
				break;
				
			case "Coin":
				Hide ();
				player.addCoins(1);
				player.HealthChange(1);
				break;
			}
			break;
		}
	}

	public void bounce( float vy, float ay )
	{
		bouncing = true;
		this.vy = vy;
		this.ay = ay;
		//originalPosition = gameObject.transform.position;
		originalPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y-.05f, gameObject.transform.position.z);
	}
	
	void Update () 
	{
		transform.rotation *= new Quaternion(0, Mathf.Sin(rotationSpeed*Time.deltaTime), 0, Mathf.Cos(rotationSpeed*Time.deltaTime));
	
		if (bouncing) 
		{
			gameObject.transform.position = new Vector3(gameObject.transform.position.x,
			                                            gameObject.transform.position.y + vy * Time.deltaTime,
			                                            gameObject.transform.position.z
			);

			vy += ay * Time.deltaTime;
			if ( gameObject.transform.position.y < originalPosition.y )
			{
				bouncing = false;
				gameObject.transform.position = new Vector3(originalPosition.x, originalPosition.y, originalPosition.z);
			}
		}
	}

	/// <seealso cref="IResettable"/>
	public void SaveState()
	{

	}

	/// <seealso cref="IResettable"/>
	public void Reset()
	{

	}
}
