using UnityEngine;
using System.Collections;

public class CoinController : MonoBehaviour {

	public float rotationSpeed;
	void Start () {
		
	}
	
	void Update () 
	{
		transform.RotateAround (transform.position, Vector3.up, rotationSpeed * Mathf.Rad2Deg * Time.deltaTime);
	}
}
