using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

/// <summary>
/// A class that configures the price of a unit.
/// </summary>
public class PriceSetter : MonoBehaviour
{
	[SerializeField] private Price price;
	[SerializeField] private float buildTime;

	/// <summary>
	/// This unit's price.
	/// </summary>
	public Price Price => price;

	public float BuildTime => buildTime;
}
