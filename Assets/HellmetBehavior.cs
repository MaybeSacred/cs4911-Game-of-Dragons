using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Controls hellmet enemy
/// </summary>
public class HellmetBehavior : GameBehaviour, IResettable 
{
	public Color angryLightColor;
	public float angryLightIntensity;
	private Color normalLightColor;
	private float normalLightIntensity;
	private Light guardLight;
	public Transform deathParticleSystem;
	public Transform editorPatrolPoints;
	public List<HellmetBehavior> friends;
	public NavMeshSettings patrolProperties;
	public NavMeshSettings attackProperties;
	public float bounceSpeed;
	public float bounciness;
	public Transform graphics;
	public Transform meshParent;
	public float yGraphicsOffset;
	private NavMeshAgent navAgent;
	private float timer;
	private int updateCounter;
	public int framesToSkip;
	public int damageToPlayer;
	private float isStunnedTimer;
	public float stunTime;
	public float playerPushForce;
	/// <summary>
	/// Together, detectionRange and detectionAngle define a sight cone in front of the hellment
	/// </summary>
	public float detectionRange;
	public float detectionAngle;
	public float foundDetectionRange;
	public float foundDetectionAngle;
	/// <summary>
	/// Defines when the bot gives up and returns to patrolling
	/// </summary>
	public float detectionTimeout;
	private float detectionTimeoutTimer;
	public float health{get; private set;}
	public float maxHealth;
	public float healthRechargeRate;
	private float deathTimeoutTimer;
	public Material meshMainTex;
	public Color heatUpColor;
	public Color coolColor;
	private Vector3[] patrolPoints;
	private int currentPatrolPoint;
	public float patrolPointTimeToWait;
	private float patrolPointWaitTimer;
	public float acceptablePatrolPointError;
	private bool foundPlayer;
	/// <summary>
	/// How long the enemy will stay around after death for cleanup animations
	/// </summary>
	public float deathTimeout;
	private Vector3 medianPoint;
	public float maxAcceptableDistFromPatrolMedian;
	private bool isUnderAttack;
	public float angerTime;
	private float theAngerTimer;

	public Transform gemFragment;
	private bool gemFragmentGiven;

	private Light resetGuardLight;
	private float resetTimer;
	private int resetUpdateCounter;
	private float resetIsStunnedTimer;
	private float resetDetectionTimeoutTimer;
	private float resetDeathTimeoutTimer;
	private Vector3[] resetPatrolPoints;
	private int resetCurrentPatrolPoint;
	private float resetPatrolPointWaitTimer;
	private bool resetFoundPlayer;
	private Vector3 resetMedianPoint;
	private bool resetIsUnderAttack;
	private float resetTheAngerTimer;
	private Vector3 resetPosition;
	private Vector3 resetRotation;
	private Vector3 resetScale;
	private Vector3 resetGraphicsPosition;
	private Vector3 resetGraphicsRotation;
	private Vector3 resetGraphicsScale;
	private Color resetTexColor;
	private float resetHealth;
	private bool resetGemFragmentGiven;

	override protected void Start()
	{
		base.Start ();

		guardLight = GetComponentInChildren<Light>();
		normalLightColor = guardLight.color;
		normalLightIntensity = guardLight.intensity;
		HealthChange(maxHealth);
		navAgent = GetComponent<NavMeshAgent>();
		meshMainTex = (Material)Instantiate(meshMainTex);
		foreach(MeshRenderer m in meshParent.GetComponentsInChildren<MeshRenderer>())
		{
			m.material = meshMainTex;
		}
		Transform[] points = editorPatrolPoints.GetComponentsInChildren<Transform>();
		RaycastHit hit;
		if(points.Length > 0)
		{
			patrolPoints = new Vector3[points.Length];
			for(int i = 0; i < points.Length; i++)
			{
				if(Physics.Raycast(points[i].position, -Vector3.up, out hit))
				{
					patrolPoints[i] = points[i].position - (new Vector3(0, hit.distance, 0));
					medianPoint += patrolPoints[i];
				}
				else
				{
					throw new UnassignedReferenceException("Error with auto patrol point generation in:" + this.name);
				}
			}
			medianPoint /= patrolPoints.Length;
		}
		else
		{
			throw new MissingReferenceException(this.name + " contains no control points");
		}
		Destroy(editorPatrolPoints.gameObject);
		meshMainTex.SetColor("_ReflectColor", coolColor);
		ReturnToCurrentPatrolPoint();

		gemFragment.gameObject.SetActive (false);
		gemFragmentGiven = false;

		SaveState ();
	}

	void OnCollisionEnter(Collision other)
	{
		
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag.Equals("Player"))
		{
			WorldScript.thePlayer.HealthChange(-damageToPlayer);
			WorldScript.thePlayer.rigidbody.AddForce((WorldScript.thePlayer.transform.position-transform.position+transform.up).normalized*playerPushForce);
			isStunnedTimer += Time.deltaTime;
		}
	}

	void OnTriggerStay(Collider other)
	{
		if(other.tag.Equals("FireBreath"))
		{
			HealthChange(-Time.deltaTime*WorldScript.thePlayer.GetAttackDamage());
			isUnderAttack = true;
			theAngerTimer = 0;
		}
	}

	void Update ()
	{
		if(deathTimeoutTimer > 0)
		{
			if(deathTimeoutTimer > deathTimeout)
			{
				Instantiate(deathParticleSystem, graphics.position, graphics.rotation);
				//Destroy(gameObject);
				Hide ();
			}
			else
			{
				guardLight.intensity = Mathf.Lerp(guardLight.intensity, 0, Time.deltaTime);
				GetComponentInChildren<ParticleSystem>().emissionRate = Mathf.Lerp(GetComponentInChildren<ParticleSystem>().emissionRate, 0, Time.deltaTime);
				deathTimeoutTimer += Time.deltaTime;
			}
		}
		else
		{
			Vector3 distanceToPlayer = WorldScript.thePlayer.transform.position-transform.position;
			if(isUnderAttack)
			{
				if(!foundPlayer)
				{
					foundPlayer = true;
					AlertFriends();
					attackProperties.SetNavMeshAgent(navAgent);
					SetAngryLight();
					navAgent.SetDestination(WorldScript.thePlayer.transform.position);
				}
				theAngerTimer += Time.deltaTime;
				if(theAngerTimer > angerTime)
				{
					isUnderAttack = false;
					foundPlayer = false;
					SetNormalLight();
					detectionTimeoutTimer = detectionTimeout;
					ReturnToCurrentPatrolPoint();
				}
				else
				{
					UpdateWhenPlayerFound(distanceToPlayer);
				}
			}
			else
			{
				if((WorldScript.thePlayer.transform.position - medianPoint).magnitude < maxAcceptableDistFromPatrolMedian)
				{
					if(distanceToPlayer.magnitude < foundDetectionRange && Vector3.Angle(distanceToPlayer, graphics.forward) < foundDetectionAngle)
					{
						if(distanceToPlayer.magnitude < detectionRange && Vector3.Angle(distanceToPlayer, graphics.forward) < detectionAngle)
						{
							if(!foundPlayer)
							{
								foundPlayer = true;
								AlertFriends();
								SetAngryLight();
								attackProperties.SetNavMeshAgent(navAgent);
								navAgent.SetDestination(WorldScript.thePlayer.transform.position);
							}
						}
					}
					else
					{
						detectionTimeoutTimer += Time.deltaTime;
						if(detectionTimeoutTimer > detectionTimeout)
						{
							foundPlayer = false;
							SetNormalLight();
						}
					}
				}
				else
				{
					foundPlayer = false;
				}
				if(foundPlayer)
				{
					UpdateWhenPlayerFound(distanceToPlayer);
				}
				else
				{
					UpdateNoPlayerFound();
				}
				HealthChange(healthRechargeRate*Time.deltaTime);
			}
			ApplyBounce();
		}
	}

	/// <summary>
	/// Controls what happens when the Hellmet knows where the player is
	/// </summary>
	/// <param name="distanceToPlayer">Distance to player.</param>
	private void UpdateWhenPlayerFound(Vector3 distanceToPlayer)
	{
		detectionTimeoutTimer = 0;
		if(isStunnedTimer > 0)
		{
			isStunnedTimer += Time.deltaTime;
			if(isStunnedTimer > stunTime)
			{
				isStunnedTimer = 0;
			}
		}
		else
		{
			MoveTowardsPlayer(distanceToPlayer);
		}
	}

	/// <summary>
	/// Controls what happens when the Hellmet does not know where the player is
	/// </summary>
	private void UpdateNoPlayerFound()
	{
		if(detectionTimeoutTimer > detectionTimeout)
		{
			PatrolPath();
		}
		else
		{
			detectionTimeoutTimer += Time.deltaTime;
			if(detectionTimeoutTimer > detectionTimeout)
			{
				ReturnToCurrentPatrolPoint();
				foundPlayer = false;
			}
		}
	}

	/// <summary>
	/// Moves Hellmet towards player.
	/// </summary>
	/// <param name="vectorToPlayer">Vector to player.</param>
	private void MoveTowardsPlayer(Vector3 vectorToPlayer)
	{
		if(updateCounter%framesToSkip ==0)
		{
			navAgent.SetDestination(WorldScript.thePlayer.transform.position);
		}
		updateCounter++;
	}

	/// <summary>
	/// Returns to current patrol point.
	/// </summary>
	private void ReturnToCurrentPatrolPoint()
	{
		patrolProperties.SetNavMeshAgent(navAgent);
		navAgent.SetDestination(patrolPoints[currentPatrolPoint]);
		patrolPointWaitTimer = patrolPointTimeToWait;
	}

	/// <summary>
	/// Patrols the path.
	/// </summary>
	private void PatrolPath()
	{
		if(patrolPointWaitTimer < patrolPointTimeToWait)
		{
			patrolPointWaitTimer += Time.deltaTime;
			if(patrolPointWaitTimer > patrolPointTimeToWait)
			{
				IncrementCurrentPatrolPoint();
				navAgent.SetDestination(patrolPoints[currentPatrolPoint]);
			}
		}
		else
		{
			Vector3 distanceToCurrentPoint = patrolPoints[currentPatrolPoint] - transform.position;
			if(distanceToCurrentPoint.magnitude < acceptablePatrolPointError)
			{
				patrolPointWaitTimer = 0;
			}
		}
	}

	/// <summary>
	/// Increments the current patrol point.
	/// </summary>
	private void IncrementCurrentPatrolPoint()
	{
		currentPatrolPoint++;
		if(currentPatrolPoint >= patrolPoints.Length)
		{
			currentPatrolPoint = 0;
		}
	}

	/// <summary>
	/// Starts death sequence
	/// </summary>
	public void KillMe()
	{
		if (!gemFragmentGiven) 
		{
			gemFragment.gameObject.SetActive( true );
			gemFragmentGiven = true;
			gemFragment.parent = null;

			CoinController cc = (CoinController)(gemFragment.GetComponent("CoinController"));
			cc.bounce( 10, Physics.gravity.y );
		}

		if(deathTimeoutTimer <= 0)
		{
			graphics.gameObject.AddComponent<Rigidbody>();
			navAgent.enabled = false;
			deathTimeoutTimer = .0001f;
		}
	}

	/// <summary>
	/// Adds deltaHealth to current health.
	/// Sets color based on health, and calls
	/// KillMe() if health is less than or 
	/// equal to 0.
	/// </summary>
	/// <param name="deltaHealth">Delta health.</param>
	public void HealthChange(float deltaHealth)
	{
		health += deltaHealth;
		if(health <=0)
		{
			health = 0;
			KillMe();
		}
		else if(health > maxHealth)
		{
			health = maxHealth;
		}
		meshMainTex.SetColor("_ReflectColor", Color.Lerp(coolColor, heatUpColor, (maxHealth-health)/maxHealth));
	}

	/// <summary>
	/// Sets the light to angry mode
	/// </summary>
	private void SetAngryLight()
	{
		guardLight.intensity = angryLightIntensity;
		guardLight.color = angryLightColor;
	}

	/// <summary>
	/// Sets the light to normal mode
	/// </summary>
	private void SetNormalLight()
	{
		guardLight.intensity = normalLightIntensity;
		guardLight.color = normalLightColor;
	}

	/// <summary>
	/// Do bounce animation
	/// </summary>
	private void ApplyBounce()
	{
		float temp = Mathf.Sin(bounceSpeed*Time.timeSinceLevelLoad);
		graphics.localPosition = new Vector3(graphics.localPosition.x, bounciness*temp*temp+yGraphicsOffset, graphics.localPosition.z);
	}

	/// <summary>
	/// Communicates to "friend" hellmets that
	/// they should attack the player.
	/// </summary>
	public void AlertFriends()
	{
		AlertMe();
		foreach(HellmetBehavior hb in friends)
		{
			hb.AlertMe();
		}
	}

	/// <summary>
	/// Tells hellmet to attack player
	/// </summary>
	public void AlertMe()
	{
		isUnderAttack = true;
		theAngerTimer = 0;
	}

	/// <seealso cref="IResettable"/> 
	public void SaveState()
	{
		resetGuardLight = guardLight;
		resetTimer = timer;
		resetUpdateCounter = updateCounter;
		resetIsStunnedTimer = isStunnedTimer;
		resetDetectionTimeoutTimer = detectionTimeoutTimer;
		resetDeathTimeoutTimer = deathTimeoutTimer;
		resetPatrolPoints = (Vector3[])patrolPoints.Clone();
		resetCurrentPatrolPoint = currentPatrolPoint;
		resetPatrolPointWaitTimer = patrolPointWaitTimer;
		resetFoundPlayer = foundPlayer;
		resetMedianPoint = medianPoint;
		resetIsUnderAttack = isUnderAttack;
		resetTheAngerTimer = theAngerTimer;
		resetPosition = transform.localPosition;
		resetRotation = transform.localEulerAngles;
		resetScale = transform.localScale;
		resetGraphicsPosition = graphics.transform.localPosition;
		resetGraphicsRotation = graphics.transform.localEulerAngles;
		resetGraphicsScale = graphics.transform.localScale;
		resetHealth = health;
		resetTexColor = meshMainTex.GetColor ("_ReflectColor");
		resetGemFragmentGiven = gemFragmentGiven;
	}

	/// <seealso cref="IResettable"/>
	public void Reset()
	{
		guardLight = resetGuardLight;
		timer = resetTimer;
		updateCounter = resetUpdateCounter;
		isStunnedTimer = resetIsStunnedTimer;
		detectionTimeoutTimer = resetDetectionTimeoutTimer;
		deathTimeoutTimer = resetDeathTimeoutTimer;
		patrolPoints = (Vector3[])resetPatrolPoints.Clone();
		currentPatrolPoint = resetCurrentPatrolPoint;
		patrolPointWaitTimer = resetPatrolPointWaitTimer;
		foundPlayer = resetFoundPlayer;
		medianPoint = resetMedianPoint;
		isUnderAttack = resetIsUnderAttack;
		theAngerTimer = resetTheAngerTimer;
		transform.localPosition = resetPosition;
		transform.localEulerAngles = resetRotation;
		transform.localScale = resetScale;
		graphics.transform.localPosition = resetGraphicsPosition;
		graphics.transform.localEulerAngles = resetGraphicsRotation;
		graphics.transform.localScale = resetGraphicsScale;
		meshMainTex.SetColor ("_ReflectColor", resetTexColor);
		health = resetHealth;
		navAgent.enabled = true;
		gemFragmentGiven = resetGemFragmentGiven;
		if (graphics.gameObject.rigidbody != null)
			Destroy(graphics.gameObject.rigidbody);
	}
}
