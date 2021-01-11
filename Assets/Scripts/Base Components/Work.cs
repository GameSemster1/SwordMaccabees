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
		var position = transform.position;
		return Vector3.Distance(position, resource.GetPointOnBounds(position)) <= range;
	}

	public void WorkAt(Resource resource)
	{
		StartCoroutine(WorkCoroutine(resource));
	}

	public void StopWorking()
	{
		IsWorking = false;
		// StopCoroutine("WorkCoroutine");
	}

	private IEnumerator WorkCoroutine(Resource resource)
	{
		if (!IsInRange(resource))
		{
			yield break;
		}

		IsWorking = true;
		if (lastWork + rate > Time.time)
		{
			yield return new WaitWhile(() => lastWork + rate > Time.time);
		}

		while (IsWorking)
		{
			if (!IsInRange(resource))
			{
				yield break;
			}

			var gathered = resource.Gathered(power, burden);

			if (gathered.IsNothing())
			{
				IsWorking = false;
				yield break;
			}

			bank.Received(gathered);
			lastWork = Time.time;

			if (!IsWorking) yield break;

			yield return new WaitForSeconds(rate);
		}
	}
}