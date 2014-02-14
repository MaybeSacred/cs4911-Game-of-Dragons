using UnityEngine;
using System.Collections;
[System.Serializable]
public class NavMeshSettings{
	public float radius, speed, acceleration, angularSpeed, stoppingDistance;
	public bool autoBraking, autoRepath;
	public void SetNavMeshAgent(NavMeshAgent input)
	{
		input.radius = radius;
		input.speed = speed;
		input.acceleration = acceleration;
		input.angularSpeed = angularSpeed;
		input.stoppingDistance = stoppingDistance;
		input.autoBraking = autoBraking;
		input.autoRepath = autoRepath;
	}
}
