using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

/// <summary>
/// A class that is in charge of a unit's movement.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class Movement : MonoBehaviour
{
	[SerializeField] [Tooltip("The unit's walking speed (in m/s).")]
	private float walkingSpeed;

	[SerializeField] [Tooltip("The unit's running speed (in m/s).")]
	private float runningSpeed;

	[SerializeField] [Tooltip("An action that will be performed whenever the unit starts moving.")]
	private UnityEvent onMove;

	[SerializeField] [Tooltip("An action that will be performed whenever the unit stops moving.")]
	private UnityEvent onStop;

	/// <summary>
	/// The Transform this unit is heading to, *If it is heading towards a transform rather than at position*.
	/// Null if the unit arrived or if it's heading towards a position.
	/// </summary>
	public Transform Target { get; private set; }

	private NavMeshAgent agent;

	private bool isRunning;

	private Action onDestinationArrive;

	/// <summary>
	/// Is this unit currently running?
	/// </summary>
	public bool IsRunning => !IsMoving && isRunning;

	/// <summary>
	/// Is this unit moving?
	/// </summary>
	public bool IsMoving { get; private set; }

	private void Start()
	{
		agent = GetComponent<NavMeshAgent>();
	}

	private void Update()
	{
		if (IsMoving && agent.remainingDistance == 0f)
		{
			IsMoving = false;
			onStop?.Invoke();
			onDestinationArrive?.Invoke();
			Target = null;
		}

		if (Target != null)
		{
			agent.SetDestination(Target.position);
		}
	}

	/// <summary>
	/// Go to a specified position.
	/// </summary>
	/// <param name="pos">The position to go to.</param>
	/// <param name="distance">The distance from 'pos' to stop at.</param>
	/// <param name="run">Should we run there?</param>
	/// <returns>True if 'pos' is reachable.</returns>
	public bool GoTo(Vector3 pos, float distance, bool run, Action whenDone = null)
	{
		isRunning = run;
		agent.speed = run ? runningSpeed : walkingSpeed;

		agent.stoppingDistance = distance;

		if (agent.SetDestination(pos))
		{
			onMove?.Invoke();
			IsMoving = true;
			onDestinationArrive = whenDone;

			return true;
		}

		return false;
	}

	/// <summary>
	/// Go to a specified Transform's position (updating every frame).
	/// </summary>
	/// <param name="trans">The Transform to go to.</param>
	/// <param name="distance">The distance from 'trans.position' to stop at.</param>
	/// <param name="run">Should we run there?</param>
	/// <returns>True if 'trans.position' is reachable. Note: this only checks in the current frame.</returns>
	public bool GoTo(Transform trans, float distance, bool run, Action whenDone = null)
	{
		Target = trans;
		isRunning = run;
		agent.speed = run ? runningSpeed : walkingSpeed;

		agent.stoppingDistance = distance;

		if (agent.SetDestination(trans.position))
		{
			onMove?.Invoke();
			IsMoving = true;
			onDestinationArrive = whenDone;
			return true;
		}

		return false;
	}

	/// <summary>
	/// Stop moving.
	/// </summary>
	public void Stop()
	{
		agent.isStopped = true;
		IsMoving = false;
	}
}