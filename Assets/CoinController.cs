using UnityEngine;
using System.Collections;

public class CoinController : MonoBehaviour {

	public float rotationSpeed;
	void Start () {
		
	}
	
	void Update () 
	{
		transform.rotation *= new Quaternion(0, Mathf.Sin(rotationSpeed*Time.deltaTime), 0, Mathf.Cos(rotationSpeed*Time.deltaTime));
	}
}
