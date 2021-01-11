using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildUI : MonoBehaviour
{
	public static BuildUI instance;

	[SerializeField] private BuildButton[] buttons;

	private Component myCaller;

	private void Awake()
	{
		instance = this;
		Disable(null);
	}

	public void Enable(BuildEntry[] entries, Component caller)
	{
		myCaller = caller;

		for (var i = 0; i < buttons.Length; i++)
		{
			if (i < entries.Length)
			{
				var entry = entries[i];

				buttons[i].SetSprite(entry.image);
				buttons[i].SetText(entry.text);
				buttons[i].SetAction(entry.action);
				buttons[i].SetProgress(() => entry.FillAmount);
				buttons[i].Enable();
			}
			else
			{
				buttons[i].Disable();
			}
		}
	}

	public void Disable(Component caller)
	{
		if (myCaller != caller)
			return;

		foreach (var button in buttons)
		{
			button.Disable();
		}
	}
}