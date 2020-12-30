using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls enemy soldiers. (currently only shows the number of enemies left).
/// </summary>
public class EnemySoldierController : SoldierController
{
	private static readonly List<EnemySoldierController> enemies = new List<EnemySoldierController>();

	private void Awake()
	{
		enemies.Add(this);
	}

	private void OnDestroy()
	{
		enemies.Remove(this);
	}


	private void OnGUI()
	{
		// GUILayout.Label($"HP: {life.hp}");
		GUILayout.Label($"Enemies Left: {enemies.Count}");
	}
}