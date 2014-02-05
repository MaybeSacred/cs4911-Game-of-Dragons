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
	void Start () {
		navAgent = GetComponent<NavMeshAgent>();
	}
	void OnCollisionEnter(Collision other)
	{
		Debug.Log("bleh");
	}
	void Update () {
		ApplyBounce();
		if(updateCounter%framesToSkip ==0)
		{
			navAgent.SetDestination(WorldScript.thePlayer.transform.position);
		}
		updateCounter++;
		graphics.rotation = Quaternion.RotateTowards(graphics.rotation, Quaternion.LookRotation(graphics.position - WorldScript.thePlayer.transform.position), Time.deltaTime*graphicsRotationSpeed);
	}
	
	private void ApplyBounce()
	{
		float temp = Mathf.Sin(bounceSpeed*Time.timeSinceLevelLoad);
		graphics.localPosition = new Vector3(graphics.localPosition.x, bounciness*temp*temp+yGraphicsOffset, graphics.localPosition.z);
	}
}
