using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoldierController : SoldierController
{
	private void OnGUI()
	{
		GUILayout.Label($"HP: {life.hp}");
	}
}