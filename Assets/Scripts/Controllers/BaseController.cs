using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseController : MonoBehaviour
{
	[SerializeField] protected Sight sight;
	[SerializeField] protected Movement movement;
	[SerializeField] protected Attack attack;
	[SerializeField] protected Nation nation;
	[SerializeField] protected Life life;

	public LayerMask attackMask;

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

	private Life ScanForEnemies()
	{
		if (attack != null)
			foreach (var other in Physics.OverlapSphere(transform.position, attack.range, attackMask))
			{
				var n = other.GetComponent<Nation>();
				if (n == null || !nation.IsHostile(n)) continue;

				var l = other.GetComponent<Life>();
				if (l == null) continue;
				return l;
			}

		foreach (var other in Physics.OverlapSphere(transform.position, sight.range, attackMask))
		{
			var n = other.GetComponent<Nation>();
			if (n == null || !nation.IsHostile(n)) continue;

			var l = other.GetComponent<Life>();
			if (l == null) continue;
			return l;
		}

		return null;
	}

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
					movement.GoTo(target.transform, attack.range, true);

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

	public bool GoTo(Vector3 position)
	{
		return movement != null && movement.GoTo(position, 0, true);
	}

	public void Attack(Life target)
	{
		StartCoroutine(AttackCoroutine(target, true));
	}
}