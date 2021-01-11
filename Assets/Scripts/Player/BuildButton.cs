using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BuildButton : MonoBehaviour
{
	[SerializeField] private Button button;
	[SerializeField] private TextMeshProUGUI text;
	[SerializeField] private Image image;
	[SerializeField] private Image progressBar;

	private Func<float> getAmount;

	private void Update()
	{
		if (getAmount != null)
			progressBar.fillAmount = getAmount.Invoke();
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
}