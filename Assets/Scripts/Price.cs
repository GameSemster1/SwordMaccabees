using System;
using UnityEngine;

/// <summary>
/// A struct that represents the price of a unit.
/// </summary>
[Serializable]
public struct Price
{
	[Tooltip("How much wood does this unit cost?")]
	public float wood;

	[Tooltip("How much rock does this unit cost?")]
	public float rock;

	[Tooltip("How much iron does this unit cost?")]
	public float iron;

	[Tooltip("How much wheat does this unit cost?")]
	public float wheat;

	[Tooltip("How much water does this unit cost?")]
	public float water;

	public static Price operator +(Price left, Price right)
	{
		return new Price
		{
			wood = left.wood + right.wood,
			rock = left.rock + right.rock,
			iron = left.iron + right.iron,
			wheat = left.wheat + right.wheat,
			water = left.water + right.water
		};
	}

	public static Price operator -(Price left, Price right)
	{
		return new Price
		{
			wood = left.wood - right.wood,
			rock = left.rock - right.rock,
			iron = left.iron - right.iron,
			wheat = left.wheat - right.wheat,
			water = left.water - right.water
		};
	}

	public static Price operator *(Price left, float right)
	{
		return new Price
		{
			wood = left.wood * right,
			rock = left.rock * right,
			iron = left.iron * right,
			wheat = left.wheat * right,
			water = left.water * right
		};
	}

	public static Price operator /(Price left, float right)
	{
		return new Price
		{
			wood = left.wood / right,
			rock = left.rock / right,
			iron = left.iron / right,
			wheat = left.wheat / right,
			water = left.water / right
		};
	}

	public static bool operator ==(Price left, Price right)
	{
		return left.wood == right.wood && left.rock == right.rock && left.iron == right.iron &&
		       left.wheat == right.wheat && left.water == right.water;
	}

	public static bool operator !=(Price left, Price right)
	{
		return !(left == right);
	}

	public bool IsNothing()
	{
		return wood <= 0 && rock <= 0 && iron <= 0 && wheat <= 0 && water <= 0;
	}

	public bool IsNegative()
	{
		return wood < 0 || rock < 0 || iron < 0 || wheat < 0 || water < 0;
	}

	public float Sum()
	{
		return wood + rock + iron + wheat + water;
	}
}