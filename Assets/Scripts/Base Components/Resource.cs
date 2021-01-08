using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Resource : MonoBehaviour
{
	[SerializeField] [Tooltip("Resource value.")]
	private Price value = new Price
	{
		wood = Mathf.Infinity, rock = Mathf.Infinity, iron = Mathf.Infinity,
		water = Mathf.Infinity, wheat = Mathf.Infinity
	};

	private Collider col;

	public Price Value => value;

	private void Start()
	{
		col = GetComponent<Collider>();
	}

	public bool IsInRange(Vector3 position)
	{
		return col.bounds.Contains(position);
	}

	/// <summary>
	/// Called when a worker gathers from this resource.
	/// Should take care of decreasing the value of this resource.
	/// </summary>
	/// <param name="power">The power of the worker.</param>
	/// <param name="max">The maximum amount of resources a this worker will take.</param>
	/// <returns>The amount the worker can gather.</returns>
	public Price Gathered(float power, float max)
	{
		var amount = value * power;

		if (amount.Sum() <= max)
		{
			SubtractResources(amount);
			return amount;
		}

		amount = amount * max / amount.Sum();

		SubtractResources(amount);

		return amount;
	}

	private void SubtractResources(Price amount)
	{
		value -= amount;
	}
}