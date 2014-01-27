using UnityEngine;
using System.Collections;

public class CoinController : MonoBehaviour {
	private Quaternion rotationQuaternion;
	public float rotationSpeed;
	void Start () {
		Debug.Log("hi");
		Debug.Log("hi");
		Debug.Log("hi");
		Debug.Log("hi");
		Debug.Log("hi");Debug.Log("hi");
		
	}
	
	void Update () {
		transform.rotation *= new Quaternion(0, Mathf.Sin(rotationSpeed*Time.deltaTime), 0, Mathf.Cos(rotationSpeed*Time.deltaTime));
	}
}
