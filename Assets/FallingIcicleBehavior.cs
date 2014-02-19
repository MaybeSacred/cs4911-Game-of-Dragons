using UnityEngine;
using System.Collections;

public class FallingIcicleBehavior : GameBehaviour {
	public float dropTimeAfterTriggered;
	public float deathTimeout;
	private float dropTimer;
	private Vector3 initialPosition;
	private Vector3 initialScale;
	private float shrinkTimer;
	public float shrinkingTime;
	public float shakingAmplitude;
	public float shakingSpeed;

	override protected void Start()
	{
		base.Start ();

		initialPosition = transform.position;
		initialScale = transform.localScale;
	}
	
	void Update () {
		if(dropTimer > 0)
		{
			if(shrinkTimer > 0)
			{
				transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, shrinkTimer);
				shrinkTimer += Time.deltaTime;
				if(shrinkTimer > shrinkingTime)
				{
					Destroy(gameObject);
				}
			}
			else
			{
				if(dropTimer < dropTimeAfterTriggered)
				{
					transform.position = initialPosition+new Vector3(Mathf.PerlinNoise(dropTimer*shakingSpeed, 0), 0, Mathf.PerlinNoise(0, shakingSpeed*dropTimer))*shakingAmplitude;
				}
				else
				{
					if(dropTimer > deathTimeout)
					{
						shrinkTimer += Time.deltaTime;
					}
					rigidbody.isKinematic = false;
				}
				dropTimer += Time.deltaTime;
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
