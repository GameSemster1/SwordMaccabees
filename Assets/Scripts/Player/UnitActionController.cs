using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionController : MonoBehaviour
{
	public UnitSelector selector;
	public Camera cam;
	public int mouseButton;
	public LayerMask actionMask;
	public float maxDistance = Mathf.Infinity;

	// Start is called before the first frame update
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButtonDown(mouseButton))
		{
			var ray = cam.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray,
				out var info, maxDistance, actionMask))
			{
				Debug.DrawRay(ray.origin, ray.direction, Color.red);

				foreach (var selectable in selector.SelectedUnits)
				{
					selectable.ActionAt(info.point, info.collider.gameObject);
				}

				// Debug.Log($"Action At: {info.point}. Object: {info.collider.name}.");
			}
		}
	}
}