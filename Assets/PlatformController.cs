using UnityEngine;
using System.Collections;

public class PlatformController : MonoBehaviour {
	private Vector3 startPosition;
	public Vector3 endPosition;
	public float movementSpeed;
	private float movementTimer;
	private float totalTimeToNextPoint;
	public float endPointWaitTime;
	private float waitTimer;
	private bool isWaiting;
	private bool movingTowardsEnd;
	void Start () {
		startPosition = transform.position;
		if(movementSpeed > 0)
		{
			totalTimeToNextPoint = endPosition.magnitude/movementSpeed;
		}
		endPosition = endPosition + transform.position;
	}
	
	void Update () 
	{
		if(isWaiting)
		{
			if(waitTimer > endPointWaitTime)
			{
				isWaiting = false;
				waitTimer = 0;
				movingTowardsEnd = !movingTowardsEnd;
			}
			else
			{
				waitTimer += Time.deltaTime;
			}
		}
		else
		{
			if(movementTimer > totalTimeToNextPoint)
			{
				isWaiting = true;
				movementTimer = 0;
			}
			else
			{
				if(movingTowardsEnd)
				{
					transform.position = Vector3.Lerp(startPosition, endPosition, movementTimer/totalTimeToNextPoint);
				}
				else
				{
					transform.position = Vector3.Lerp(endPosition, startPosition, movementTimer/totalTimeToNextPoint);
				}
				movementTimer += Time.deltaTime;
			}
		}
	}
}
