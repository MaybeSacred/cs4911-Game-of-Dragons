using UnityEngine;
using System.Collections;

public class SnowmanController : MonoBehaviour 
{
	public Transform graphics;

	public Transform topSphere;
	public Transform middleSphere;
	public Transform bottomSphere;

	public Transform rightBranch;
	public Transform hand;
	public Transform snowBall;

	public float throwDistance;

	private float throwTimer;
	public float secondsBetweenThrows;
	public float secondsBetweenReset;
	private float originalArmAngle;
	private float curAngle;
	private float throwSpeed;
	public float maxThrowSpeed;
	private SnowmanPartController top;
	private SnowmanPartController middle;
	private SnowmanPartController bottom;
	void Start () 
	{
		throwTimer = 0;
		originalArmAngle = rightBranch.transform.localEulerAngles.y;
		curAngle = 0;
		throwSpeed = 0;
		
		top = (SnowmanPartController)(topSphere.GetComponent("SnowmanPartController"));
		middle = (SnowmanPartController)(middleSphere.GetComponent("SnowmanPartController"));
		bottom = (SnowmanPartController)(bottomSphere.GetComponent("SnowmanPartController"));
	}

	void OnCollisionStay(Collision other)
	{

	}

	void Update()
	{
		graphics.transform.LookAt (WorldScript.thePlayer.transform);
		graphics.transform.localEulerAngles = new Vector3 (0, graphics.transform.localEulerAngles.y, 0);

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
			if (top == null && middle == null && bottom == null)
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

						float targetX = WorldScript.thePlayer.transform.position.x;
						float targetY = WorldScript.thePlayer.transform.position.y;
						float targetZ = WorldScript.thePlayer.transform.position.z;

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
						sc.isDangerous = true;
					}
				}
			}
			else
			{
				snowBall.transform.position = hand.position;
			}

			if ( Vector3.Distance(transform.position, WorldScript.thePlayer.transform.position) < throwDistance )
				throwTimer += Time.deltaTime;
		}
	}
}
