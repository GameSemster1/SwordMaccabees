using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class that configures the price of a unit.
/// </summary>
public class PriceSetter : MonoBehaviour
{
	[SerializeField] private Price price;

	/// <summary>
	/// This unit's price.
	/// </summary>
	public Price Price => price;
}

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
}