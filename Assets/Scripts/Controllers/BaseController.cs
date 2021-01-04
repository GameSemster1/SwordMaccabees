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
	protected Stance stance = Stance.Guard;

	private bool isAttacking;
	private bool isGuarding;
	private bool isAutoAttacking;

	private bool isBored = true;

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

	private bool destroy;

	// Update is called once per frame
	protected virtual void Update()
	{
		if (destroy)
			Destroy(gameObject);

		if (life.IsDead)
			destroy = true;

		if (isBored && !isAutoAttacking)
		{
			switch (stance)
			{
				case Stance.Aggressive:
					StartCoroutine(AggressiveCoroutine());
					break;
				case Stance.Guard:
					StartCoroutine(GuardCoroutine());
					break;
				case Stance.HoldPosition:
					StartCoroutine(HoldPositionCoroutine());
					break;
				case Stance.HoldFire:
					break;
			}
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
				if (!other.TryGetComponent<Nation>(out var n) || !nation.IsHostile(n))
					continue;

				if (!other.TryGetComponent<Life>(out var l) || l.IsDead) continue;

				if (additionalCondition != null && !additionalCondition(l)) continue;

				target = l;
				return true;
			}

		foreach (var other in Physics.OverlapSphere(transform.position, sight.Range, attackMask))
		{
			if (!other.TryGetComponent<Nation>(out var n) || !nation.IsHostile(n)) continue;

			if (!other.TryGetComponent<Life>(out var l) || l.IsDead) continue;

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
		anchor = transform.position;
		isGuarding = true;
		isAutoAttacking = true;
		maxDistanceFromAnchor = sight.Range - attack.Range;
		while (isGuarding && isAutoAttacking && isBored)
		{
			if (ScanForEnemies(out var target,
				l => Vector3.Distance(l.transform.position, anchor) < maxDistanceFromAnchor))
			{
				yield return StartCoroutine(AttackCoroutine(target, true, true));
			}
			else yield return null;

			if (isBored && isAutoAttacking)
				movement.GoTo(anchor, 0, true);
		}

		isAutoAttacking = false;
	}

	private IEnumerator AggressiveCoroutine()
	{
		if (!canSee || !canAttack)
			yield break;


		isAutoAttacking = true;

		while (isAutoAttacking && isBored)
		{
			if (ScanForEnemies(out var target))
			{
				yield return StartCoroutine(AttackCoroutine(target, true, false));
			}
			else yield return null;
		}

		isAutoAttacking = false;
	}

	private IEnumerator HoldPositionCoroutine()
	{
		if (!canSee || !canAttack)
			yield break;

		isAutoAttacking = true;
		while (isAutoAttacking && isBored)
		{
			if (ScanForEnemies(out var target))
			{
				yield return StartCoroutine(AttackCoroutine(target, false, false));
			}
			else yield return null;
		}

		isAutoAttacking = false;
	}

	/// <summary>
	/// A Coroutine that attacks a target.
	/// </summary>
	/// <param name="target">A 'Life' object to attack.</param>
	/// <param name="chase">Should we chase 'target' if it runs away?</param>
	/// <param name="useAnchor"></param>
	private IEnumerator AttackCoroutine(Life target, bool chase, bool useAnchor, Action whenDone = null)
	{
		if (!canAttack || target == null || target.IsDead)
		{
			isAttacking = false;
			whenDone?.Invoke();
			yield break;
		}

		isAttacking = true;

		while (isAttacking)
		{
			if (target.IsDead)
				break;

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
					whenDone?.Invoke();
					yield break;
				}
			}

			attack.StartAttacking(target);

			yield return new WaitWhile(() => attack.IsAttacking);

			if (!chase || target == null || target.IsDead)
			{
				isAttacking = false;
				whenDone?.Invoke();
				yield break;
			}
		}

		whenDone?.Invoke();
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
		isBored = false;
		isAutoAttacking = false;
		StartCoroutine(InvokeNextFrame(() =>
		{
			StopAttacking();
			StartCoroutine(InvokeNextFrame(() => { movement.GoTo(position, 0, true, () => { isBored = true; }); }));
		}));
		return true;
	}

	/// <summary>
	/// Tells this unit to go to an object on the map.
	/// </summary>
	/// <param name="trans">The Transform to go to.</param>
	/// <returns>True if 'trans' can currently be reached.</returns>
	public bool GoTo(Transform trans)
	{
		if (!canMove) return false;
		isBored = false;
		isAutoAttacking = false;

		StartCoroutine(InvokeNextFrame(() =>
		{
			StopAttacking();
			StartCoroutine(InvokeNextFrame(() => { movement.GoTo(trans, 0, true, () => { isBored = true; }); }));
		}));

		return true;
	}

	private IEnumerator InvokeNextFrame(Action action)
	{
		yield return null;
		action?.Invoke();
	}

	/// <summary>
	/// Tells this unit to attack a target.
	/// </summary>
	/// <param name="target">The target to attack.</param>
	public bool Attack(Life target)
	{
		if (!canAttack) return false;

		if (!canMove && !attack.IsInRange(target.transform.position)) return false;

		isBored = false;

		StartCoroutine(InvokeNextFrame(() =>
		{
			StartCoroutine(AttackCoroutine(target, true, false, () => { isBored = true; }));
		}));


		return true;
	}

	public void StopAttacking()
	{
		isAttacking = false;
		attack.StopAttacking();
	}
}