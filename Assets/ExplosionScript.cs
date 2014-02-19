using UnityEngine;
using System.Collections;

public class ExplosionScript : GameBehaviour {
	private float destructionTimer;
	private ParticleSystem ps;

	override protected void Start()
	{
		base.Start ();

		ps = GetComponent<ParticleSystem>();
	}
	
	void Update () {
		destructionTimer += Time.deltaTime;
		if(destructionTimer > ps.duration + ps.startLifetime)
		{
			Destroy(gameObject);
		}
	}
}
