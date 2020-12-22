using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
	public Sight sight;
	public Movement movement;
	public Attack attack;
	public Nation nation;

	public LayerMask mask;

	public bool chase;

	private bool isAttacking;

	// Start is called before the first frame update
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
		if (!isAttacking)
		{
			StartCoroutine(AttackCoroutine(ScanForEnemies()));
		}
	}

	private Life ScanForEnemies()
	{
		foreach (var other in Physics.OverlapSphere(transform.position, sight.range, mask))
		{
			var n = other.GetComponent<Nation>();
			if (n == null || !nation.IsHostile(n)) continue;

			var l = other.GetComponent<Life>();
			if (l == null) continue;
			return l;
		}

		return null;
	}

	private IEnumerator AttackCoroutine(Life target)
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
				movement.GoTo(target.transform.position, attack.range, true);

				yield return new WaitUntil(() => attack.IsInRange(target.transform.position) && movement.IsRunning);
			}
			else
			{
				isAttacking = false;
				yield break;
			}
		}

		attack.StartAttacking(target);

		yield return new WaitWhile(() => attack.IsAttacking);
		var newTarget = ScanForEnemies();
		StartCoroutine(AttackCoroutine(newTarget));
	}

	public bool GoTo(Vector3 position)
	{
		return movement != null && movement.GoTo(position, 0, true);
	}

	public void Attack(Life target)
	{
		StartCoroutine(AttackCoroutine(target));
	}
}