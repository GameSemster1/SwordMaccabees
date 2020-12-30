using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A class that is in charge of a unit's attack.
/// </summary>
public class Attack : MonoBehaviour
{
	[SerializeField] [Tooltip("This unit's attack range.")]
	private float range;

	[SerializeField] [Tooltip("This unit's attack rate.")]
	private float rate;

	[SerializeField] [Tooltip("This unit's attack strength.")]
	private float power;

	[SerializeField] [Tooltip("This unit's attack's armor intrusion.")]
	private float intrusion;

	[SerializeField] [Tooltip("A mask of the objects that this unit can't attack through them.")]
	private LayerMask obstacleMask;

	[SerializeField]
	[Tooltip(
		"A tag that all obstacles have. A unit can't attack through an object that is in the obstacle mask and has the obstacle tag.")]
	private string obstacleTag = "Attack Obstacle";

	[SerializeField] [Tooltip("An action that will be performed every time this unit attacks.")]
	private UnityEvent onAttack;

	[SerializeField] [Tooltip("An action that will be performed whenever this unit stops attacking completely.")]
	private UnityEvent onStop;

	/// <summary>
	/// This unit's attack range.
	/// </summary>
	public float Range => range;

	/// <summary>
	/// This unit's attack rate.
	/// </summary>
	public float Rate => rate;

	/// <summary>
	/// This unit's attack strength.
	/// </summary>
	public float Power => power;

	/// <summary>
	/// This unit's attack's armor intrusion.
	/// </summary>
	public float Intrusion => intrusion;

	private float lastAttack;

	public bool IsAttacking { get; private set; }

	private IEnumerator AttackCoroutine(Life target)
	{
		IsAttacking = true;
		if (lastAttack + rate > Time.time)
		{
			yield return new WaitWhile(() => lastAttack + rate > Time.time);
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
			onAttack?.Invoke();
			if (target.WasHit(this) == 0)
			{
				IsAttacking = false;
				break;
			}

			yield return new WaitForSeconds(rate);
		}

		onStop?.Invoke();
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

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, range);
	}

	/// <summary>
	/// Adds an action that will be performed evey time this unit attacks.
	/// </summary>
	/// <param name="action">The action to perform.</param>
	public void DoOnAttack(UnityAction action)
	{
		onAttack.AddListener(action);
	}

	/// <summary>
	/// Adds an action that will be performed whenever this unit stops attacking completely.
	/// </summary>
	/// <param name="action">The action to perform.</param>
	public void DoOnStop(UnityAction action)
	{
		onStop.AddListener(action);
	}
}