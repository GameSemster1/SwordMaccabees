using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] private Button button;
	[SerializeField] private TextMeshProUGUI text;
	[SerializeField] private Image image;
	[SerializeField] private Image progressBar;

	private Func<float> getAmount;
	private Tooltip tooltip;

	private Func<string> tooltipText;

	private bool mouseOver;

	private void Update()
	{
		if (getAmount != null)
			progressBar.fillAmount = getAmount.Invoke();

		if (mouseOver)
			tooltip.UpdatePosition(Input.mousePosition);
	}

	public void SetTooltipObject(Tooltip tooltip)
	{
		this.tooltip = tooltip;
	}

	public void SetTooltip(Func<string> tooltipText)
	{
		this.tooltipText = tooltipText;
	}

	public void SetText(string t)
	{
		text.text = t;
	}

	public void SetSprite(Sprite s)
	{
		image.sprite = s;
	}

	public void SetAction(Action a)
	{
		button.onClick.RemoveAllListeners();
		button.onClick.AddListener(new UnityAction(a));
	}

	public void SetProgress(Func<float> getAmount)
	{
		this.getAmount = getAmount;
	}

	public void Disable()
	{
		gameObject.SetActive(false);
	}

	public void Enable()
	{
		gameObject.SetActive(true);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		mouseOver = true;
		tooltip.ShowTooltip(tooltipText?.Invoke(), Input.mousePosition);
		// Debug.Log("Mouse enter");
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		mouseOver = false;
		tooltip.HideTooltip();
		// Debug.Log("Mouse exit");
	}
}