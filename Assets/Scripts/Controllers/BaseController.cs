using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A class that controls a unit, implementing auto attack.
/// </summary>
public class BaseController : MonoBehaviour
{
	[SerializeField] [Tooltip("This unit's 'Sight' component.")]
	protected Sight sight;

	[SerializeField] [Tooltip("This unit's 'Movement' component.")]
	protected Movement movement;

	[SerializeField] [Tooltip("This unit's 'Attack' component.")]
	protected Attack attack;

	[SerializeField] [Tooltip("This unit's 'Nation' component.")]
	protected Nation nation;

	[SerializeField] [Tooltip("This unit's 'Life' component.")]
	protected Life life;

	[SerializeField] [Tooltip("A mask of the units this unit can attack.")]
	private LayerMask attackMask;

	[SerializeField] [Tooltip("The default stance of this unit.")]
	protected Stance stance;

	private bool isAttacking;
	private bool isGuarding;
	private Vector3 anchor;
	private float maxDistanceFromAnchor;

	private bool canMove;
	private bool canAttack;
	private bool canSee;

	public enum Stance
	{
		Aggressive,
		Guard,
		HoldPosition,
		HoldFire
	}

	protected virtual void Awake()
	{
	}

	protected virtual void Start()
	{
		canMove = movement != null;
		canAttack = attack != null;
		canSee = sight != null;
	}

	// Update is called once per frame
	protected virtual void Update()
	{
		if (life.IsDead)
			Destroy(gameObject);

		if (!isAttacking && stance != Stance.HoldFire)
		{
			anchor = transform.position;
			if (canSee && canAttack)
				maxDistanceFromAnchor = sight.Range - attack.Range;
			if (ScanForEnemies(out var target))
				StartCoroutine(AttackCoroutine(target,
					stance == Stance.Aggressive, stance == Stance.Guard));
		}
	}

	/// <summary>
	/// Finds an enemy we can attack.
	/// </summary>
	/// <param name="target"></param>
	/// <param name="additionalCondition"></param>
	/// <returns></returns>
	private bool ScanForEnemies(out Life target, Func<Life, bool> additionalCondition = null)
	{
		if (canAttack)
			foreach (var other in Physics.OverlapSphere(transform.position, attack.Range, attackMask))
			{
				if (!TryGetComponent<Nation>(out var n) || !nation.IsHostile(n)) continue;

				if (!TryGetComponent<Life>(out var l) || l.IsDead) continue;

				if (additionalCondition != null && !additionalCondition(l)) continue;

				target = l;
				return true;
			}

		foreach (var other in Physics.OverlapSphere(transform.position, sight.Range, attackMask))
		{
			if (!TryGetComponent<Nation>(out var n) || !nation.IsHostile(n)) continue;

			if (!TryGetComponent<Life>(out var l) || l.IsDead) continue;

			if (additionalCondition != null && !additionalCondition(l)) continue;

			target = l;
			return true;
		}

		target = null;
		return false;
	}

	private IEnumerator GuardCoroutine()
	{
		if (!canSee || !canAttack)
			yield break;
		isGuarding = true;
		maxDistanceFromAnchor = sight.Range - attack.Range;
		while (isGuarding && stance == Stance.Guard)
		{
			GoTo(anchor);
			if (ScanForEnemies(out var target,
				l => Vector3.Distance(l.transform.position, anchor) < maxDistanceFromAnchor))
			{
				yield return StartCoroutine(AttackCoroutine(target, true, true));
			}
		}
	}

	/// <summary>
	/// A Coroutine that attacks a target.
	/// </summary>
	/// <param name="target">A 'Life' object to attack.</param>
	/// <param name="chase">Should we chase 'target' if it runs away?</param>
	/// <param name="useAnchor"></param>
	private IEnumerator AttackCoroutine(Life target, bool chase, bool useAnchor)
	{
		if (!canAttack || target == null || target.IsDead)
		{
			isAttacking = false;
			yield break;
		}

		isAttacking = true;

		while (isAttacking)
		{
			if (!attack.IsInRange(target.transform.position))
			{
				if (!canMove)
				{
					isAttacking = false;
					yield break;
				}

				if (useAnchor)
					yield return StartCoroutine(Chase(target, anchor, maxDistanceFromAnchor));
				else
					yield return StartCoroutine(Chase(target));

				if (!attack.IsInRange(target.transform.position))
				{
					isAttacking = false;
					yield break;
				}
			}

			attack.StartAttacking(target);

			yield return new WaitWhile(() => attack.IsAttacking);

			if (!chase || target == null || target.IsDead)
			{
				isAttacking = false;
				yield break;
			}
		}
	}

	private IEnumerator Chase(Life target)
	{
		yield return StartCoroutine(Chase(target, default, Mathf.Infinity));
	}

	private IEnumerator Chase(Life target, Vector3 anchor, float maxDistance)
	{
		movement.GoTo(target.transform, attack.Range, true);

		int returnReason = 0;

		yield return new WaitUntil(() =>
		{
			if (target == null || target.IsDead)
			{
				returnReason = 1;
				return true;
			}

			if (attack.IsInRange(target.transform.position))
			{
				returnReason = 2;
				return true;
			}

			if (Vector3.Distance(target.transform.position, anchor) > maxDistance)
			{
				returnReason = 3;
				return true;
			}

			return false;
		});
	}

	/// <summary>
	/// Tells this unit to go to a position on the map.
	/// </summary>
	/// <param name="position">The position to go to.</param>
	/// <returns>True if 'position can be reached.</returns>
	public bool GoTo(Vector3 position)
	{
		if (!canMove) return false;
		StopAttacking();
		return movement.GoTo(position, 0, true);
	}

	/// <summary>
	/// Tells this unit to go to an object on the map.
	/// </summary>
	/// <param name="trans">The Transform to go to.</param>
	/// <returns>True if 'trans' can currently be reached.</returns>
	public bool GoTo(Transform trans)
	{
		if (!canMove) return false;
		StopAttacking();
		return movement.GoTo(trans, 0, true);
	}

	/// <summary>
	/// Tells this unit to attack a target.
	/// </summary>
	/// <param name="target">The target to attack.</param>
	public bool Attack(Life target)
	{
		if (!canAttack) return false;

		if (!canMove && !attack.IsInRange(target.transform.position)) return false;

		StartCoroutine(AttackCoroutine(target, true, false));

		return true;
	}

	public void StopAttacking()
	{
		isAttacking = false;
		attack.StopAttacking();
	}
}