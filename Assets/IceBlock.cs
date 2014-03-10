using UnityEngine;
using System.Collections;

/// <summary>
/// Controls meltable blocks of ice.
/// </summary>
public class IceBlock : GameBehaviour, IResettable 
{
	private Vector3 initialScale;
	private float currentScaleMagnitude;
	public float minAcceptableSize;
	public float meltRate;

	private Vector3 resetPosition;
	private Vector3 resetRotation;
	private Vector3 resetScale;

	override protected void Start()
	{
		base.Start ();

		initialScale = transform.localScale;
		currentScaleMagnitude = initialScale.magnitude;

		SaveState ();
	}

	void OnTriggerStay(Collider other)
	{
		if(other.tag.Equals("FireBreath"))
		{
			currentScaleMagnitude -= WorldScript.thePlayer.GetAttackDamage()*Time.deltaTime*meltRate;
			transform.localScale = Vector3.Lerp(Vector3.zero, initialScale, currentScaleMagnitude/initialScale.magnitude);
			if(currentScaleMagnitude < minAcceptableSize)
			{
				Hide ();
			}
		}
	}

	void Update () 
	{
	
	}

	/// <seealso cref="IResettable"/>
	public void SaveState()
	{
		resetPosition = transform.localPosition;
		resetRotation = transform.localEulerAngles;
		resetScale = transform.localScale;
	}

	/// <seealso cref="IResettable"/>
	public void Reset()
	{
		transform.localPosition = resetPosition;
		transform.localEulerAngles = resetRotation;
		transform.localScale = resetScale;
		currentScaleMagnitude = initialScale.magnitude;
	}
}
