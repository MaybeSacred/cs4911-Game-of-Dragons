using UnityEngine;
using System.Collections;

/// <summary>
/// Controls an enemy snowman that throws snowballs at the player.
/// </summary>
public class SnowmanController : GameBehaviour, IResettable
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

	public Transform gemFragment;
	private bool gemFragmentGiven;  // can't just disable fragment since fragment hides when collected

	private float resetThrowTimer;
	private float resetCurAngle;
	private float resetThrowSpeed;
	private Vector3 resetRotation;
	private Vector3 resetGraphicsRotation;
	private Vector3 resetBranchRotation;
	private Vector3 resetBranchPosition;
	private Transform resetTop;
	private Transform resetMiddle;
	private Transform resetBottom;

	override protected void Start()
	{
		base.Start ();

		throwTimer = 0;
		originalArmAngle = rightBranch.transform.localEulerAngles.y;
		curAngle = 0;
		throwSpeed = 0;
		
		top = (SnowmanPartController)(topSphere.GetComponent("SnowmanPartController"));
		middle = (SnowmanPartController)(middleSphere.GetComponent("SnowmanPartController"));
		bottom = (SnowmanPartController)(bottomSphere.GetComponent("SnowmanPartController"));

		gemFragment.gameObject.SetActive (false);
		gemFragmentGiven = false;

		SaveState ();
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
			if ( !gemFragmentGiven )
			{
				gemFragment.gameObject.SetActive( true );
				gemFragmentGiven = true;
				gemFragment.parent = null;

				CoinController cc = (CoinController)(gemFragment.GetComponent("CoinController"));
				cc.bounce( 10, Physics.gravity.y );
			}

			if (snowBall != null)
			{
				snowBall.parent = null;	
				snowBall.rigidbody.isKinematic = false;
				SnowballController sc = (SnowballController)(snowBall.GetComponent("SnowballController"));
				sc.setStartMelting();
			}

			if (top.gameObject.activeSelf)
				top.DeathShrink();
			if (middle.gameObject.activeSelf)
				middle.DeathShrink();
			if (bottom.gameObject.activeSelf)
				bottom.DeathShrink();

			if (gameObject.activeSelf && !top.gameObject.activeSelf && !middle.gameObject.activeSelf && !bottom.gameObject.activeSelf)
				Hide ();
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
					
						resetSnowball();
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

	/// <seealso cref="IResettable"/>
	public void SaveState()
	{
		resetThrowTimer = throwTimer;
		resetCurAngle = curAngle;
		resetThrowSpeed = throwSpeed;
		resetRotation = transform.localEulerAngles;
		resetGraphicsRotation = graphics.transform.localEulerAngles;
		resetBranchRotation = rightBranch.transform.localEulerAngles;
		resetBranchPosition = rightBranch.transform.localPosition;
		resetTop = topSphere;
		resetMiddle = middleSphere;
		resetBottom = bottomSphere;
	}

	/// <seealso cref="IResettable"/>
	public void Reset()
	{
		throwTimer = resetThrowTimer;
		curAngle = resetCurAngle;
		throwSpeed = resetThrowSpeed;
		transform.localEulerAngles = resetRotation;
		graphics.transform.localEulerAngles = resetGraphicsRotation;
		rightBranch.transform.localEulerAngles = resetBranchRotation;
		rightBranch.transform.localPosition = resetBranchPosition;
		topSphere = resetTop;
		middleSphere = resetMiddle;
		bottomSphere = resetBottom;
		resetSnowball ();
	}

	private void resetSnowball()
	{
		SnowballController sc = (SnowballController)(snowBall.GetComponent("SnowballController"));
		sc.setStartMelting();
		snowBall = Instantiate(snowBall, hand.position, hand.rotation) as Transform;
		snowBall.rigidbody.isKinematic = true;
		snowBall.localScale = new Vector3(1, 1, 1);
		sc = (SnowballController)(snowBall.GetComponent("SnowballController"));
		sc.isDangerous = true;
	}
}
