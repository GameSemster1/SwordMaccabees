using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBank : MonoBehaviour
{
	private static Dictionary<Nation.Type, PlayerBank> banks = new Dictionary<Nation.Type, PlayerBank>();

	public static PlayerBank Get(Nation nation)
	{
		return banks[nation.NationType];
	}

	[SerializeField] private Nation nation;

	public Price CurrentResources { get; private set; }

	private void Awake()
	{
		banks.Add(nation.NationType, this);
	}

	public bool Buy(Price price)
	{
		var res = CurrentResources - price;

		if (res.IsNegative())
		{
			return false;
		}

		CurrentResources = res;
		return true;
	}

	public void Received(Price amount)
	{
		CurrentResources += amount;
	}
}