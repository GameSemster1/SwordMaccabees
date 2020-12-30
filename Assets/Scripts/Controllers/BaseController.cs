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

	private bool isAttacking;

	// Update is called once per frame
	private void Update()
	{
		if (life.IsDead)
			Destroy(gameObject);

		if (!isAttacking)
		{
			StartCoroutine(AttackCoroutine(ScanForEnemies(), false));
		}
	}

	/// <summary>
	/// Finds an enemy we can attack.
	/// </summary>
	private Life ScanForEnemies()
	{
		if (attack != null)
			foreach (var other in Physics.OverlapSphere(transform.position, attack.Range, attackMask))
			{
				var n = other.GetComponent<Nation>();
				if (n == null || !nation.IsHostile(n)) continue;

				var l = other.GetComponent<Life>();
				if (l == null) continue;
				return l;
			}

		foreach (var other in Physics.OverlapSphere(transform.position, sight.Range, attackMask))
		{
			var n = other.GetComponent<Nation>();
			if (n == null || !nation.IsHostile(n)) continue;

			var l = other.GetComponent<Life>();
			if (l == null) continue;
			return l;
		}

		return null;
	}

	/// <summary>
	/// A Coroutine that attacks a target.
	/// </summary>
	/// <param name="target">A 'Life' object to attack.</param>
	/// <param name="chase">Should we chase 'target' if it runs away?</param>
	private IEnumerator AttackCoroutine(Life target, bool chase)
	{
		while (true)
		{
			if (target == null || attack == null)
			{
				isAttacking = false;
				yield break;
			}


			isAttacking = true;

			if (!attack.IsInRange(target.transform.position))
			{
				if (movement != null)
				{
					movement.GoTo(target.transform, attack.Range, true);

					yield return new WaitUntil(() =>
					{
						if (target == null || target.IsDead)
						{
							return true;
						}

						return attack.IsInRange(target.transform.position) && movement.IsRunning;
					});
					if (target == null || target.IsDead)
					{
						isAttacking = false;
						yield break;
					}
				}
				else
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

	/// <summary>
	/// Tells this unit to go to a position on the map.
	/// </summary>
	/// <param name="position">The position to go to.</param>
	/// <returns>True if 'position can be reached.</returns>
	public bool GoTo(Vector3 position)
	{
		return movement != null && movement.GoTo(position, 0, true);
	}

	/// <summary>
	/// Tells this unit to go to an object on the map.
	/// </summary>
	/// <param name="trans">The Transform to go to.</param>
	/// <returns>True if 'trans' can currently be reached.</returns>
	public bool GoTo(Transform trans)
	{
		return movement != null && movement.GoTo(trans, 0, true);
	}

	/// <summary>
	/// Tells this unit to attack a target.
	/// </summary>
	/// <param name="target">The target to attack.</param>
	public void Attack(Life target)
	{
		StartCoroutine(AttackCoroutine(target, true));
	}
}