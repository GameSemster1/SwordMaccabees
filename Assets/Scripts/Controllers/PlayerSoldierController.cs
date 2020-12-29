using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoldierController : SoldierController, ISelectable
{
	public GameObject selectionCircle;

	private new Collider collider;

	public Bounds Bounds
	{
		get
		{
			if (collider == null)
			{
				collider = GetComponent<Collider>();
			}

			return collider.bounds;
		}
	}

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
		Debug.Log($"Action At {position}");
		var n = obj.GetComponent<Nation>();
		if (n != null)
		{
			if (nation.IsHostile(n))
			{
				var l = obj.GetComponent<Life>();
				if (l != null)
				{
					Attack(l);
					return;
				}
			}
		}

		if (!GoTo(position))
		{
			Debug.Log($"{name} cant reach {position}.");
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.magenta;
		if (IsSelected)
			Gizmos.DrawWireSphere(transform.position, 10);
	}
}