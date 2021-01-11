using System;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class PlayerBank : MonoBehaviour
{
	private static Dictionary<Nation.Type, PlayerBank> banks = new Dictionary<Nation.Type, PlayerBank>();

	public static PlayerBank Get(Nation nation)
	{
		return banks[nation.NationType];
	}

	[SerializeField] private Nation nation;
	[SerializeField] private Price startingResources;

	// [SerializeField] private TextMeshProUGUI wood, rock, iron, wheat, water;

	[SerializeField] private TextMeshProUGUI field;
	[SerializeField] private int totalResouceLength = 4;
	[SerializeField] private string woodPrefix, rockPrefix, ironPrefix, wheatPrefix, waterPrefix;

	public Price CurrentResources { get; private set; }

	private void Awake()
	{
		banks.Add(nation.NationType, this);
		CurrentResources = startingResources;

		UpdateResourceValues();
	}

	private void Update()
	{
		UpdateResourceValues();
	}

	private void UpdateResourceValues()
	{
		// wood.text = woodPrefix + CurrentResources.wood;
		// rock.text = rockPrefix + CurrentResources.rock;
		// iron.text = ironPrefix + CurrentResources.iron;
		// wheat.text = wheatPrefix + CurrentResources.wheat;
		// water.text = waterPrefix + CurrentResources.water;

		var wood = CurrentResources.wood.ToString(CultureInfo.CurrentCulture).PadLeft(totalResouceLength);
		var rock = CurrentResources.rock.ToString(CultureInfo.CurrentCulture).PadLeft(totalResouceLength);
		var iron = CurrentResources.iron.ToString(CultureInfo.CurrentCulture).PadLeft(totalResouceLength);
		var wheat = CurrentResources.wheat.ToString(CultureInfo.CurrentCulture).PadLeft(totalResouceLength);
		var water = CurrentResources.water.ToString(CultureInfo.CurrentCulture).PadLeft(totalResouceLength);
		
		field.text = $"{woodPrefix}{wood}. {rockPrefix}{rock}. {ironPrefix}{iron}. {wheatPrefix}{wheat}. {waterPrefix}{water}.";
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