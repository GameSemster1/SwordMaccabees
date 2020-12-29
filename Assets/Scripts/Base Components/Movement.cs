using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public class Movement : MonoBehaviour
{
	public float walkingSpeed;
	public float runningSpeed;

	public UnityEvent onMove, onStop;

	public Transform target;

	private NavMeshAgent agent;

	private bool isRunning;
	private bool isMoving;

	public bool IsRunning => !agent.isStopped && isRunning;

	private void Start()
	{
		agent = GetComponent<NavMeshAgent>();
	}

	private void Update()
	{
		if (isMoving && agent.isStopped)
		{
			isMoving = false;
			onStop?.Invoke();
			target = null;
		}

		if (target != null)
		{
			agent.SetDestination(target.position);
		}
	}

	public bool GoTo(Vector3 pos, float distance, bool run)
	{
		isRunning = run;
		agent.speed = run ? runningSpeed : walkingSpeed;

		agent.stoppingDistance = distance;

		if (agent.SetDestination(pos))
		{
			onMove?.Invoke();
			isMoving = true;
			return true;
		}

		return false;
	}

	public bool GoTo(Transform trans, float distance, bool run)
	{
		target = trans;
		isRunning = run;
		agent.speed = run ? runningSpeed : walkingSpeed;

		agent.stoppingDistance = distance;

		if (agent.SetDestination(trans.position))
		{
			onMove?.Invoke();
			isMoving = true;
			return true;
		}

		return false;
	}

	public void Stop()
	{
		agent.isStopped = true;
	}
}