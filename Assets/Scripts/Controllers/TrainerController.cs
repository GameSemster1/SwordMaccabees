using System;
using System.Collections;
using UnityEngine;

public class TrainerController : MonoBehaviour, ISelectable
{
	[SerializeField] private Nation nation;

	[SerializeField] private Transform exitFlag;

	[SerializeField] private Entry[] soldiers;

	[SerializeField] private GameObject selectionCircle;

	[Serializable]
	public class Entry : BuildEntry
	{
		public PriceSetter soldier;

		[HideInInspector] public float startBuildTime;
		[HideInInspector] public bool isTraining = false;

		public override float FillAmount => isTraining ? (Time.time - startBuildTime) / soldier.BuildTime : 1;
	}

	private PlayerBank bank;
	private Collider col;

	// Start is called before the first frame update
	void Start()
	{
		bank = PlayerBank.Get(nation);
		col = GetComponent<Collider>();

		foreach (var t in soldiers)
		{
			t.action = () => { Train(t); };
		}
	}

	// Update is called once per frame
	void Update()
	{
	}

	public bool Train(Entry entry)
	{
		if (!bank.Buy(entry.soldier.Price)) return false;
		entry.isTraining = true;
		entry.startBuildTime = Time.time;

		StartCoroutine(TrainCoroutine(entry));

		return true;
	}

	private IEnumerator TrainCoroutine(Entry entry)
	{
		yield return new WaitForSeconds(entry.soldier.BuildTime);
		entry.isTraining = false;
		var newSolider = Instantiate(entry.soldier, transform.position, Quaternion.identity);

		if (newSolider.TryGetComponent<BaseController>(out var c))
		{
			yield return null;
			c.GoTo(exitFlag.position);
		}
		else if (newSolider.TryGetComponent<Movement>(out var m))
		{
			yield return null;
			m.GoTo(exitFlag.position, 0, true);
		}
	}

	public Bounds Bounds => col.bounds;
	public bool IsSelected { get; private set; }

	public void OnHighlight(bool isHighlighted)
	{
	}

	public void OnSelect(bool dragSelect)
	{
		if (dragSelect)
		{
			OnDeselect();
			return;
		}

		IsSelected = true;
		selectionCircle.SetActive(true);

		exitFlag.gameObject.SetActive(true);

		BuildUI.instance.Enable(soldiers, this);
	}

	public void OnDeselect()
	{
		IsSelected = false;
		selectionCircle.SetActive(false);
		exitFlag.gameObject.SetActive(false);
		BuildUI.instance.Disable(this);
	}

	public void ActionAt(Vector3 position, GameObject obj)
	{
		exitFlag.position = position;
	}
}