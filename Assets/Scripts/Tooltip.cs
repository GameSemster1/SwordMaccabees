using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI text;
	[SerializeField] private RectTransform background;

	[SerializeField] private float padding;

	[SerializeField] private RectTransform trans;
	[SerializeField] private int idleFrameNumber = 5;


	private int lastUpdateFrame;
	private bool isShown;

	private void Awake()
	{
		// trans = GetComponent<RectTransform>();

		if (Time.frameCount - lastUpdateFrame > idleFrameNumber && isShown)
			HideTooltip();
	}

	public void ShowTooltip(string tooltipText, Vector2 position)
	{
		text.text = tooltipText;

		var size = new Vector2(text.preferredWidth + padding * 2, text.preferredHeight + padding * 2);
		background.sizeDelta = size;

		gameObject.SetActive(true);
		UpdatePosition(position);

		lastUpdateFrame = Time.frameCount;
		isShown = true;
	}

	public void UpdatePosition(Vector2 position)
	{
		Vector2 movement = Vector2.zero;

		var size = background.sizeDelta;

		if (position.x < 0)
		{
			movement.x += 0 - position.x;
		}

		if (position.x + size.x > Screen.width)
		{
			movement.x +=  Screen.width - (position.x + size.x);
		}

		if (position.y < 0)
		{
			movement.y += 0 - position.y;
		}

		if (position.y + size.y > Screen.height)
		{
			movement.y += Screen.height - (position.y + size.y);
		}

		Debug.Log(movement);

		trans.anchoredPosition = position + movement;

		lastUpdateFrame = Time.frameCount;
	}

	public void HideTooltip()
	{
		gameObject.SetActive(false);
		isShown = false;
	}
}