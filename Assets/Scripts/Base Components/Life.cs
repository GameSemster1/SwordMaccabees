using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Life : MonoBehaviour
{
	public float hp;
	public float armor;

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

	public bool IsDead => hp <= 0;
}