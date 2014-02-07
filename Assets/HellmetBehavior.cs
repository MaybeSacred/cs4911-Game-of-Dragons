using UnityEngine;
using System.Collections;

public class HellmetBehavior : MonoBehaviour {
	public float bounceSpeed;
	public float bounciness;
	public Transform graphics;
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
	void Start () {
		navAgent = GetComponent<NavMeshAgent>();
	}
	void OnCollisionEnter(Collision other)
	{
		Debug.Log("bleh");
	}
	void OnTriggerEnter(Collider other)
	{
		if(other.tag.Equals("Player"))
		{
			WorldScript.thePlayer.HealthChange(-damageToPlayer);
			WorldScript.thePlayer.rigidbody.AddForce((WorldScript.thePlayer.transform.position-transform.position).normalized*playerPushForce);
			isStunnedTimer += Time.deltaTime;
		}
	}
	void Update () {
		ApplyBounce();
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
			if(updateCounter%framesToSkip ==0)
			{
				navAgent.SetDestination(WorldScript.thePlayer.transform.position);
			}
			updateCounter++;
		}
		graphics.rotation = Quaternion.RotateTowards(graphics.rotation, Quaternion.LookRotation(graphics.position - WorldScript.thePlayer.transform.position), Time.deltaTime*graphicsRotationSpeed);
	}
	
	private void ApplyBounce()
	{
		float temp = Mathf.Sin(bounceSpeed*Time.timeSinceLevelLoad);
		graphics.localPosition = new Vector3(graphics.localPosition.x, bounciness*temp*temp+yGraphicsOffset, graphics.localPosition.z);
	}
}
