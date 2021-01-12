using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PartA : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI[] firstTexts;
	[SerializeField] private TextMeshProUGUI[] secondTexts;
	[SerializeField] private KeyCode next = KeyCode.Space;

	[SerializeField] private Movement[] workers;
	[SerializeField] private TrainerController trainBase;

	[SerializeField] private float distanceFromBase;
	[SerializeField] private int numberOfSoldiers;

	[SerializeField] private string nextLevel;

	private int currentIndex = 0;

	private bool trainedSoldiers;
	private bool arrivedAtBase;

	private void Start()
	{
		trainBase.DoOnTrain(OnTrain);

		for (var i = 0; i < firstTexts.Length; i++)
		{
			firstTexts[i].gameObject.SetActive(i == currentIndex);
		}

		for (var i = 0; i < secondTexts.Length; i++)
		{
			secondTexts[i].gameObject.SetActive(false);
		}
	}

	private void Update()
	{
		if (!trainedSoldiers && !arrivedAtBase)
		{
			if (Input.GetKeyDown(next) && currentIndex < firstTexts.Length)
			{
				currentIndex++;
				for (var i = 0; i < firstTexts.Length; i++)
				{
					firstTexts[i].gameObject.SetActive(i == currentIndex);
				}
			}

			var arrived = true;

			foreach (var worker in workers)
			{
				if (Vector3.Distance(worker.transform.position, trainBase.transform.position) > distanceFromBase)
				{
					arrived = false;
				}
			}

			if (arrived)
			{
				arrivedAtBase = true;
				OnArrivedAtBase();
			}
		}
		else if (!trainedSoldiers)
		{
			if (Input.GetKeyDown(next) && currentIndex < secondTexts.Length)
			{
				currentIndex++;
				for (var i = 0; i < secondTexts.Length; i++)
				{
					secondTexts[i].gameObject.SetActive(i == currentIndex);
				}
			}
		}
		else
		{
			SceneManager.LoadScene(nextLevel);
		}
	}

	private void OnArrivedAtBase()
	{
		foreach (var t in firstTexts)
		{
			t.gameObject.SetActive(false);
		}

		currentIndex = 0;
		for (var i = 0; i < secondTexts.Length; i++)
		{
			secondTexts[i].gameObject.SetActive(i == currentIndex);
		}
	}

	private int soldiers = 0;

	private void OnTrain()
	{
		soldiers++;

		if (soldiers >= numberOfSoldiers)
		{
			trainedSoldiers = true;

			foreach (var t in firstTexts)
			{
				t.gameObject.SetActive(false);
			}

			foreach (var t in secondTexts)
			{
				t.gameObject.SetActive(false);
			}
		}
	}
}