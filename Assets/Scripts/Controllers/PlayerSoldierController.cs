using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoldierController : SoldierController, ISelectable
{
	public Bounds Bounds => GetComponent<Renderer>().bounds;
	public bool IsSelected { get; private set; }

	public void OnSelect(bool dragSelect)
	{
		IsSelected = true;
	}

	public void OnDeselect()
	{
		IsSelected = false;
	}

	public void ActionAt(Vector3 position, GameObject obj)
	{
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

		GoTo(position);
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.magenta;
		if (IsSelected)
			Gizmos.DrawWireSphere(transform.position, 10);
	}
}