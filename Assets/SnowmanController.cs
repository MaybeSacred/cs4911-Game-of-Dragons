using UnityEngine;
using System.Collections;

public class SnowmanController : MonoBehaviour 
{
	public Transform objectToLookAt;	

	public Transform topSphere;
	public Transform middleSphere;
	public Transform bottomSphere;

	void Start () 
	{

	}

	void OnCollisionStay(Collision other)
	{

	}
	
	void Update()
	{
		transform.LookAt (objectToLookAt);
		transform.localEulerAngles = new Vector3 (0, transform.localEulerAngles.y, 0);

		SnowballController top = null;
		if (topSphere != null) top = (SnowballController)(topSphere.GetComponent("SnowballController"));
		SnowballController middle = null;
		if (middleSphere != null) middle = (SnowballController)(middleSphere.GetComponent("SnowballController"));
		SnowballController bottom = null;
		if (bottomSphere != null) bottom = (SnowballController)(bottomSphere.GetComponent("SnowballController"));

		bool isDead = (top == null || top.isDead ()) || (middle == null || middle.isDead ()) || (bottom == null || bottom.isDead ());
		if (isDead) 
		{
			if (top != null)
				top.DeathShrink();
			if (middle != null)
				middle.DeathShrink();
			if (bottom != null)
				bottom.DeathShrink();
		}
	}
}
