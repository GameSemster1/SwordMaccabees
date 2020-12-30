using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class that represents this unit's field of view.
/// </summary>
public class Sight : MonoBehaviour
{
	[SerializeField] private float range;

	/// <summary>
	/// The range of sight of this object.
	/// </summary>
	public float Range => range;

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, range);
	}
}