using UnityEngine;
using System.Collections.Generic;

public class HellmetBehavior : GameBehaviour {
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
	public float graphicsRotationSpeed;
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
				Destroy(gameObject);
			}
			guardLight.intensity = Mathf.Lerp(guardLight.intensity, 0, Time.deltaTime);
			GetComponentInChildren<ParticleSystem>().emissionRate = Mathf.Lerp(GetComponentInChildren<ParticleSystem>().emissionRate, 0, Time.deltaTime);
			deathTimeoutTimer += Time.deltaTime;
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
			}
			HealthChange(healthRechargeRate*Time.deltaTime);
			ApplyBounce();
		}
	}
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
	private void MoveTowardsPlayer(Vector3 vectorToPlayer)
	{
		if(updateCounter%framesToSkip ==0)
		{
			navAgent.SetDestination(WorldScript.thePlayer.transform.position);
		}
		updateCounter++;
	}
	private void ReturnToCurrentPatrolPoint()
	{
		patrolProperties.SetNavMeshAgent(navAgent);
		navAgent.SetDestination(patrolPoints[currentPatrolPoint]);
		patrolPointWaitTimer = patrolPointTimeToWait;
	}
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
	private void IncrementCurrentPatrolPoint()
	{
		currentPatrolPoint++;
		if(currentPatrolPoint >= patrolPoints.Length)
		{
			currentPatrolPoint = 0;
		}
	}
	public void KillMe()
	{
		if(deathTimeoutTimer <= 0)
		{
			graphics.gameObject.AddComponent<Rigidbody>();
			navAgent.enabled = false;
			deathTimeoutTimer = .0001f;
		}
	}
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
	private void SetAngryLight()
	{
		guardLight.intensity = angryLightIntensity;
		guardLight.color = angryLightColor;
	}
	private void SetNormalLight()
	{
		guardLight.intensity = normalLightIntensity;
		guardLight.color = normalLightColor;
	}
	private void ApplyBounce()
	{
		float temp = Mathf.Sin(bounceSpeed*Time.timeSinceLevelLoad);
		graphics.localPosition = new Vector3(graphics.localPosition.x, bounciness*temp*temp+yGraphicsOffset, graphics.localPosition.z);
	}
	public void AlertFriends()
	{
		AlertMe();
		foreach(HellmetBehavior hb in friends)
		{
			hb.AlertMe();
		}
	}
	public void AlertMe()
	{
		isUnderAttack = true;
		theAngerTimer = 0;
	}
}
