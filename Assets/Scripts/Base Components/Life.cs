using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A class that is in charge of the units' life.
/// </summary>
public class Life : MonoBehaviour
{
	public float hp;
	public float armor;

	[Tooltip("Called whenever this unit is hit.")]
	public UnityEvent onHit;

	public float WasHit(Attack attacker)
	{
		hp -= attacker.power * attacker.intrusion / armor;
		onHit?.Invoke();

		if (hp <= 0)
		{
			hp = 0;
			// TODO dead
		}

		return hp;
	}

	/// <summary>
	/// Is this unit dead?
	/// </summary>
	public bool IsDead => hp <= 0;
}