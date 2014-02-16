using UnityEngine;
using System.Collections;

public class IceBlock : MonoBehaviour {
	private Vector3 initialScale;
	private float currentScaleMagnitude;
	public float minAcceptableSize;
	public float meltRate;
	void Start () {
		initialScale = transform.localScale;
		currentScaleMagnitude = initialScale.magnitude;
	}
	void OnTriggerStay(Collider other)
	{
		if(other.tag.Equals("FireBreath"))
		{
			currentScaleMagnitude -= WorldScript.thePlayer.GetAttackDamage()*Time.deltaTime*meltRate;
			transform.localScale = Vector3.Lerp(Vector3.zero, initialScale, currentScaleMagnitude/initialScale.magnitude);
			if(currentScaleMagnitude < minAcceptableSize)
			{
				Destroy(gameObject);
			}
		}
	}
	void Update () {
	
	}
}
