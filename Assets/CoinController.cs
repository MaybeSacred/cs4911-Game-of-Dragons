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
	
	void Update () 
	{
		transform.rotation *= new Quaternion(0, Mathf.Sin(rotationSpeed*Time.deltaTime), 0, Mathf.Cos(rotationSpeed*Time.deltaTime));
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
