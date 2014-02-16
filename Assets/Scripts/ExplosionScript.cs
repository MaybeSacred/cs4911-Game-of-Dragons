using UnityEngine;
using System.Collections;

public class ExplosionScript : MonoBehaviour {
	private float destructionTimer;
	private ParticleSystem ps;
	void Start () {
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
