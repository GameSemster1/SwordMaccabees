using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoldierController : SoldierController, ISelectable
{
	public bool selected;
	public Bounds Bounds => GetComponent<Renderer>().bounds;
	public bool IsSelected { get; private set; }

	public void OnSelect(bool dragSelect)
	{
		IsSelected = true;
		selected = true;
	}

	public void OnDeselect()
	{
		IsSelected = false;
		selected = false;
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
		if (selected)
			Gizmos.DrawWireSphere(transform.position, 10);
	}
}