using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWorkerController : MonoBehaviour, ISelectable
{
	[SerializeField] private Work work;
	[SerializeField] private Movement move;
	[SerializeField] private GameObject selectionCircle;

	private Collider col;

	private void Awake()
	{
		col = GetComponent<Collider>();
	}

	public Bounds Bounds => col.bounds;
	public bool IsSelected { get; private set; }

	public void OnHighlight(bool isHighlighted)
	{
	}

	public void OnSelect(bool dragSelect)
	{
		IsSelected = true;
		selectionCircle.SetActive(true);
	}

	public void OnDeselect()
	{
		IsSelected = false;
		selectionCircle.SetActive(false);
	}

	public void ActionAt(Vector3 position, GameObject obj)
	{
		if (obj.TryGetComponent<Resource>(out var r))
		{
			work.StopWorking();
			StartCoroutine(WorkCoroutine(r));
		}
		else
		{
			work.StopWorking();
			move.GoTo(position, 0, true);
		}
	}

	private IEnumerator WorkCoroutine(Resource resource)
	{
		var point = resource.GetPointOnBounds(transform.position);
		
		move.GoTo(point, 0, true);
		yield return new WaitWhile(() => move.IsMoving);
		work.WorkAt(resource);
	}
}