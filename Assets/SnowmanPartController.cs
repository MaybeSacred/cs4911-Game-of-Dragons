using UnityEngine;
using System.Collections;

public class SnowmanPartController : MonoBehaviour 
{
	private float startScale;

	void Start () 
	{
		startScale = transform.localScale.x;
	}
	
	void OnTriggerStay(Collider other)
	{
		switch (other.gameObject.tag) 
		{
		case "FireBreath":
			PlayerController pc = (PlayerController)(GameObject.FindGameObjectWithTag("Player").GetComponent("PlayerController"));
			transform.localScale = new Vector3(
				transform.localScale.x - .9f * pc.getFlameScale() * Time.deltaTime, 
				transform.localScale.y - .9f * pc.getFlameScale() * Time.deltaTime,
				transform.localScale.z - .9f * pc.getFlameScale() * Time.deltaTime
			);
			break;
		}
	}

	public bool isDead()
	{
		return transform.localScale.x < startScale * .5;
	}

	public void DeathShrink()
	{
		transform.localScale = new Vector3(
			transform.localScale.x - .2f * Time.deltaTime, 
			transform.localScale.y - .2f * Time.deltaTime,
			transform.localScale.z - .2f * Time.deltaTime
		);

		rigidbody.constraints = RigidbodyConstraints.None;
		transform.parent = null;
		
		if (transform.localScale.x < 0)
			Destroy(gameObject);
	}

	void Update()
	{

	}
}
