using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Nation))]
public class Work : MonoBehaviour
{
	[SerializeField] private float power;
	[SerializeField] private float rate;
	[SerializeField] private float burden;
	[SerializeField] private float range;

	public bool IsWorking { get; private set; }
	private float lastWork;

	private PlayerBank bank;

	private void Start()
	{
		bank = PlayerBank.Get(GetComponent<Nation>());
	}

	public bool IsInRange(Resource resource)
	{
		return resource.IsInRange(transform.position);
	}

	public void WorkAt(Resource resource)
	{
		StartCoroutine(WorkCoroutine(resource));
	}

	private IEnumerator WorkCoroutine(Resource resource)
	{
		IsWorking = true;
		if (lastWork + rate > Time.time)
		{
			yield return new WaitWhile(() => lastWork + rate > Time.time);
		}

		while (IsWorking)
		{
			if (IsInRange(resource))
			{
				bank.Received(resource.Gathered(power, burden));
			}

			yield return new WaitForSeconds(rate);
		}
	}
}