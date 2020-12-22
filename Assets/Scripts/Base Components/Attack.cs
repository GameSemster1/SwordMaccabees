﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
	public float range;
	public float rate;
	public float power;
	public float intrusion;

	public LayerMask obstacleMask;
	public string obstacleTag;

	private float lastAttack;

	public bool IsAttacking { get; private set; }

	private IEnumerator AttackCoroutine(Life target)
	{
		IsAttacking = true;
		if (lastAttack + rate < Time.time)
		{
			yield return new WaitWhile(() => lastAttack + rate < Time.time);
		}

		while (IsAttacking)
		{
			if (Physics.Raycast(transform.position, target.transform.position - transform.position,
				out var info, range, obstacleMask))
			{
				if (info.collider.CompareTag(obstacleTag))
				{
					IsAttacking = false;
					break;
				}
			}

			if (!IsInRange(target.transform.position))
			{
				IsAttacking = false;
				break;
			}

			lastAttack = Time.time;
			if (target.WasHit(this) == 0)
			{
				IsAttacking = false;
				break;
			}

			yield return new WaitForSeconds(rate);
		}
	}

	public void StartAttacking(Life target)
	{
		StartCoroutine(AttackCoroutine(target));
	}

	public void StopAttacking()
	{
		IsAttacking = false;
	}

	public bool IsInRange(Vector3 other)
	{
		return Vector3.Distance(other, transform.position) <= range;
	}
}