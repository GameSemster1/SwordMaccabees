using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the Player's soldiers.
/// </summary>
public class PlayerSoldierController : SoldierController, ISelectable
{
	[SerializeField]
	[Tooltip("An object that will be activated whenever this unit is selected (and deactivated when deselected).")]
	private GameObject selectionCircle;

	private Collider col;

	/// <summary>
	/// The bounds of this unit's collider (for selection).
	/// </summary>
	public Bounds Bounds
	{
		get
		{
			if (col == null)
			{
				col = GetComponent<Collider>();
			}

			return col.bounds;
		}
	}

	/// <summary>
	/// Is this unit selected?
	/// </summary>
	public bool IsSelected { get; private set; }

	public void OnSelect(bool dragSelect)
	{
		IsSelected = true;
		selectionCircle.SetActive(true);
	}

	public void OnDeselect()
	{
		IsSelected = false;
		selectionCircle.SetActive(false);
	}

	public void OnHighlight(bool isHighlighted)
	{
	}

	public void ActionAt(Vector3 position, GameObject obj)
	{
		if (TryGetComponent<Nation>(out var n) && nation.IsHostile(n))
		{
			if (TryGetComponent<Life>(out var l))
			{
				if (!Attack(l))
				{
					Debug.Log($"{name} can't attack {obj.name}");
				}
				else return;
			}
		}

		if (!GoTo(position))
		{
			Debug.Log($"{name} can't reach {position}.");
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.magenta;
		if (IsSelected)
			Gizmos.DrawWireSphere(transform.position, 10);
	}
}