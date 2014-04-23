using UnityEngine;
using System.Collections;

/// <summary>
/// Controls the spinning and collection behavior of all token
/// objects. This includes things that are not coins, such as
/// gems.
/// </summary>
public class CoinController : GameBehaviour, IResettable 
{

	public static int totalLargeGems = 0;
	public static int totalSmallGems = 0;

	public float rotationSpeed;

	private Vector3 originalPosition;

	private bool bouncing = false;
	private float vy = 0;
	private float ay = 0;

	private bool initialized = false;
	

	void Awake()
	{
		if (!initialized) {
			if (gameObject.tag.Equals ("Gem"))
				totalLargeGems++;
			else if (gameObject.tag.Equals ("SmallGem"))
				totalSmallGems++;
			
			initialized = true;
		}
	}

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

	/// <summary>
	/// Bounce the coint with a given velocity and acceleration
	/// </summary>
	/// <param name="vy">Initial y velocity of the bounce animation</param>
	/// <param name="ay">y acceleration of the bounce animation</param>
	public void bounce( float vy, float ay )
	{
		bouncing = true;
		this.vy = vy;
		this.ay = ay;
		originalPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y-.05f, gameObject.transform.position.z);
	}

	/// <summary>
	/// Do the rotation and bounce animations
	/// </summary>
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
		// don't save anything to reset
	}

	/// <seealso cref="IResettable"/>
	public void Reset()
	{
		// don't do anything to reset
	}
}
