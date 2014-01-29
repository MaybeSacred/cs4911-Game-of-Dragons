using UnityEngine;
using System.Collections;

public class FallingIcicleBehavior : MonoBehaviour {
	public float dropTimeAfterTriggered;
	private float dropTimer;
	private Vector3 initialPosition;
	public float shakingAmplitude;
	public float shakingSpeed;
	void Start () {
		initialPosition = transform.position;
	}
	
	void Update () {
		if(dropTimer > 0 && dropTimer < dropTimeAfterTriggered)
		{
			transform.position = initialPosition+new Vector3(Mathf.PerlinNoise(dropTimer*shakingSpeed, 0), 0, Mathf.PerlinNoise(0, shakingSpeed*dropTimer))*shakingAmplitude;
			dropTimer += Time.deltaTime;
			if(dropTimer >= dropTimeAfterTriggered)
			{
				rigidbody.isKinematic = false;
			}
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
