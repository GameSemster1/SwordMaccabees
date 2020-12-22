using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{
	public float hp;
	public float armor;

	public float WasHit(Attack attacker)
	{
		hp -= attacker.power * attacker.intrusion / armor;

		if (hp <= 0)
		{
			hp = 0;
			// TODO dead
		}

		return hp;
	}
}