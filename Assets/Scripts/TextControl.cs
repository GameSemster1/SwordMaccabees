using UnityEngine;

public class TextControl : MonoBehaviour
{
	[SerializeField] private GameObject[] list;
	[SerializeField] private KeyCode next = KeyCode.Space;

	private int currentIndex = 0;

	private void Start()
	{
		for (var i = 0; i < list.Length; i++)
		{
			list[i].SetActive(i == currentIndex);
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(next) && currentIndex < list.Length)
		{
			currentIndex++;
			for (var i = 0; i < list.Length; i++)
			{
				list[i].SetActive(i == currentIndex);
			}
		}
	}
}