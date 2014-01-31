using UnityEngine;
using System.Collections;

public class FallingIcicleBehavior : MonoBehaviour {
	public float dropTimeAfterTriggered;
	public float deathTimeout;
	private float dropTimer;
	private Vector3 initialPosition;
	public float shakingAmplitude;
	public float shakingSpeed;
	void Start () {
		initialPosition = transform.position;
	}
	
	void Update () {
		if(dropTimer > 0)
		{
			if(dropTimer < dropTimeAfterTriggered)
			{
				transform.position = initialPosition+new Vector3(Mathf.PerlinNoise(dropTimer*shakingSpeed, 0), 0, Mathf.PerlinNoise(0, shakingSpeed*dropTimer))*shakingAmplitude;
			}
			else
			{
				if(dropTimer > deathTimeout)
				{
					Destroy(gameObject);
				}
				rigidbody.isKinematic = false;
			}
			dropTimer += Time.deltaTime;
		}
	}
	void OnTriggerEnter(Collider other)
	{
		if(other.tag.Equals("Player"))
		{
			dropTimer += Time.deltaTime;
		}
	}
}
