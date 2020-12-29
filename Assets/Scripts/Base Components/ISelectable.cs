using System;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectable
{
	// public static readonly List<ISelectable> selectables = new List<ISelectable>();
	//
	// private void Awake()
	// {
	// 	selectables.Add(this);
	// }

	Bounds Bounds { get; }

	bool IsSelected { get; }

	void OnHighlight(bool isHighlighted);
	void OnSelect(bool dragSelect);
	void OnDeselect();

	void ActionAt(Vector3 position, GameObject obj);
}