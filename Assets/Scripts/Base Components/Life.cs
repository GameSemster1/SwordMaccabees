using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A class that is in charge of the units' life.
/// </summary>
public class Life : MonoBehaviour
{
	[SerializeField] [Tooltip("This unit's Hit Points.")]
	private float hp;

	[SerializeField] [Tooltip("This unit's armor (as a fraction).")]
	private float armor;

	[SerializeField] [Tooltip("Called whenever this unit is hit.")]
	private UnityEvent onHit;

	private bool destroy;

	private void Update()
	{
		if (destroy)
			Destroy(gameObject);
	}

	/// <summary>
	/// Hit points left.
	/// </summary>
	public float HP => hp;

	/// <summary>
	/// Call whenever this unit is hit.
	/// </summary>
	/// <param name="attacker">The 'Attack' that hit this unit.</param>
	/// <returns>Hp left</returns>
	public float WasHit(Attack attacker)
	{
		hp -= attacker.Power * attacker.Intrusion / armor;
		onHit?.Invoke();

		if (hp <= 0)
		{
			hp = 0;
			destroy = true;
		}

		return hp;
	}

	/// <summary>
	/// Is this unit dead?
	/// </summary>
	public bool IsDead => hp <= 0;

	/// <summary>
	/// Add an action that will be called whenever this unit is hit.
	/// </summary>
	/// <param name="action"></param>
	public void DoOnHit(UnityAction action)
	{
		onHit.AddListener(action);
	}
}