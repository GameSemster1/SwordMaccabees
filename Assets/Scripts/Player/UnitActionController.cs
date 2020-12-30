using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class that is in charge of sending the user's commands to his units.
/// </summary>
public class UnitActionController : MonoBehaviour
{
	[Tooltip("A UnitSelector who's selected units we want to control.")] [SerializeField]
	private UnitSelector selector;

	[Tooltip("The user's camera (main camera).")] [SerializeField]
	private Camera cam;

	[SerializeField] [Tooltip("The mouse button (0, 1, or 2) for sending commands.")]
	private int mouseButton;

	[SerializeField] [Tooltip("A mask of the places / units that a unit can reach / attack.")]
	private LayerMask actionMask;

	[SerializeField] [Tooltip("The distance to check for an object in the mask. ")]
	private float maxDistance = Mathf.Infinity;

	private void Update()
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