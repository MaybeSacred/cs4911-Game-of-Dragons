using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Controls the opening of the castle gate.
/// </summary>
public class GateBehaviour : MonoBehaviour 
{
	private const float MESHSWITCHZ = 1.191f; // the position at which to switch to the smaller gate mesh

	public Mesh halfGate;
	private bool activated;
	public float openingSpeed;
	public float maxZPosition;

	void Start () {
	
	}
	
	void Update () 
	{
		if(activated)
		{
			if(transform.localPosition.z < maxZPosition)
			{
				transform.localPosition += new Vector3(0, 0, openingSpeed * Time.deltaTime);
				if(halfGate != null && transform.localPosition.z > MESHSWITCHZ)
				{
					MeshFilter mf = GetComponent<MeshFilter>();
					mf.mesh = halfGate;
					halfGate = null;
				}
			}
		}
	}

	/// <summary>
	/// Make the gate begin to raise if has not already
	/// </summary>
	public void Activate()
	{
		activated = true;
	}
}
