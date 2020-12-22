using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Movement : MonoBehaviour
{
	public float walkingSpeed;
	public float runningSpeed;

	private NavMeshAgent agent;

	private bool isRunning;

	public bool IsRunning => !agent.isStopped && isRunning;

	private void Start()
	{
		agent = GetComponent<NavMeshAgent>();
	}

	public bool GoTo(Vector3 target, float distance, bool run)
	{
		isRunning = run;
		agent.speed = run ? runningSpeed : walkingSpeed;

		agent.stoppingDistance = distance;

		return agent.SetDestination(target);
	}

	public void Stop()
	{
		agent.isStopped = true;
	}
}