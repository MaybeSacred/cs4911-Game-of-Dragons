using UnityEngine;
using System.Collections;

public class SnowmanController : MonoBehaviour 
{
	public Transform objectToLookAt;	

	public Transform graphics;

	public Transform topSphere;
	public Transform middleSphere;
	public Transform bottomSphere;

	public Transform rightBranch;
	public Transform hand;
	public Transform snowBall;

	public float throwDistance = 45;

	private float throwTimer;
	private float secondsBetweenThrows;
	private float secondsBetweenReset;
	private float originalArmAngle;
	private float curAngle;
	private float throwSpeed;
	private float maxThrowSpeed;

	void Start () 
	{
		throwTimer = 0;
		secondsBetweenThrows = 3;
		secondsBetweenReset = 5;
		originalArmAngle = rightBranch.transform.localEulerAngles.y;
		curAngle = 0;
		throwSpeed = 0;
		maxThrowSpeed = 15;
	}

	void OnCollisionStay(Collision other)
	{

	}

	void Update()
	{
		graphics.transform.LookAt (objectToLookAt);
		graphics.transform.localEulerAngles = new Vector3 (0, graphics.transform.localEulerAngles.y, 0);

		SnowmanPartController top = null;
		if (topSphere != null) top = (SnowmanPartController)(topSphere.GetComponent("SnowmanPartController"));
		SnowmanPartController middle = null;
		if (middleSphere != null) middle = (SnowmanPartController)(middleSphere.GetComponent("SnowmanPartController"));
		SnowmanPartController bottom = null;
		if (bottomSphere != null) bottom = (SnowmanPartController)(bottomSphere.GetComponent("SnowmanPartController"));

		bool isDead = (top == null || top.isDead ()) || (middle == null || middle.isDead ()) || (bottom == null || bottom.isDead ());
		if (isDead) 
		{
			if (snowBall != null)
			{
				snowBall.rigidbody.isKinematic = false;
				snowBall.parent = null;
			}

			if (top != null)
				top.DeathShrink ();
			if (middle != null)
				middle.DeathShrink ();
			if (bottom != null)
				bottom.DeathShrink ();

			if (snowBall != null)
			{
				snowBall.rigidbody.isKinematic = false;
				SnowballController sc = (SnowballController)(snowBall.GetComponent("SnowballController"));
				sc.setStartMelting();
			}

			bool allGone = top == null && middle == null && bottom == null;
			if (allGone)
			{
				Destroy (gameObject);
			}
		} 
		else 
		{
			float throwAngle = 140;

			if (throwTimer > secondsBetweenThrows)
			{
				if (curAngle < throwAngle)
				{
					throwSpeed += 2;
					if (throwSpeed > maxThrowSpeed)
						throwSpeed = maxThrowSpeed;
					curAngle += throwSpeed;
					rightBranch.transform.RotateAround(middleSphere.transform.position, Vector3.up, -throwSpeed);

					snowBall.transform.position = hand.position;
				}
				else
				{
					if (snowBall.rigidbody.isKinematic) // launch snowball
					{
						snowBall.rigidbody.isKinematic = false;

						float targetX = objectToLookAt.transform.position.x;
						float targetY = objectToLookAt.transform.position.y;
						float targetZ = objectToLookAt.transform.position.z;

						float dx = Mathf.Sqrt( Mathf.Pow(targetX - hand.transform.position.x, 2) + Mathf.Pow(targetZ - hand.transform.position.z, 2)  );
						float dy = targetY - hand.transform.position.y;

						// this is not correct trajectory, but it is good enough for now
						snowBall.rigidbody.velocity = graphics.transform.forward * dx * 2 + graphics.transform.up * dy * 2;
					}

					if (throwTimer > secondsBetweenReset) // reset arm and snowball
					{
						throwTimer = 0;
						curAngle = 0;
						rightBranch.transform.localEulerAngles = new Vector3(rightBranch.transform.localEulerAngles.x, originalArmAngle, rightBranch.transform.localEulerAngles.z);
					
						SnowballController sc = (SnowballController)(snowBall.GetComponent("SnowballController"));
						sc.setStartMelting();
						snowBall = Instantiate(snowBall, hand.position, hand.rotation) as Transform;
						snowBall.rigidbody.isKinematic = true;
						snowBall.localScale = new Vector3(1, 1, 1);
						sc = (SnowballController)(snowBall.GetComponent("SnowballController"));
						sc.setIsDangerous(true);
					}
				}
			}
			else
			{
				snowBall.transform.position = hand.position;
			}

			if ( Vector3.Distance(transform.position, objectToLookAt.position) < throwDistance )
				throwTimer += Time.deltaTime;
		}
	}
}
