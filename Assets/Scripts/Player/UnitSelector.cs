using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Code based on: https://www.habrador.com/tutorials/select-units-within-rectangle/
/// </summary>
public class UnitSelector : MonoBehaviour
{
	public Camera cam;

	public int mouseButton;
	public RectTransform selectionTransform;

	public LayerMask selectionMask;

	private Vector2 startSelectionDrag, endSelectionDrag;
	private Rect selectionBox;

	private ISelectable[] selectedUnits = new ISelectable[0];

	public IEnumerable<ISelectable> SelectedUnits =>
		selectedUnits.Where(selectable => selectable != null && selectable.IsSelected);

	public ISelectable HighlightedUnit { get; private set; }

	private bool isDragging = false;

	private void Update()
	{
		if (Input.GetMouseButtonDown(mouseButton))
		{
			startSelectionDrag = Input.mousePosition;
			// isDragging = true; // TODO: fix when not dragging.
		}

		else if (Input.GetMouseButton(mouseButton))
		{
			Vector2 currentSelectionDrag = Input.mousePosition;

			if (isDragging || currentSelectionDrag != startSelectionDrag)
			{
				isDragging = true;
				selectionTransform.gameObject.SetActive(true);
				selectionBox = GetRect(startSelectionDrag, currentSelectionDrag);

				SetRectTransformFromRect(selectionTransform, selectionBox);
			}
		}
		else
		{
			var ray = cam.ScreenPointToRay(startSelectionDrag);
			if (Physics.Raycast(ray, out var info, Mathf.Infinity, selectionMask))
			{
				var s = info.collider.GetComponent<ISelectable>();

				if (s != null)
				{
					if (HighlightedUnit != s)
					{
						HighlightedUnit?.OnHighlight(false);

						s.OnHighlight(true);
						HighlightedUnit = s;
					}
				}
				else
				{
					HighlightedUnit?.OnHighlight(false);
					HighlightedUnit = null;
				}
			}
			else
			{
				HighlightedUnit?.OnHighlight(false);
				HighlightedUnit = null;
			}
		}


		if (Input.GetMouseButtonUp(mouseButton))
		{
			if (isDragging)
			{
				isDragging = false;

				selectionTransform.gameObject.SetActive(false);
				endSelectionDrag = Input.mousePosition;
				selectionBox = GetRect(startSelectionDrag, endSelectionDrag);
				ChangeSelected(GetUnitsInRect(selectionBox, cam), true);
			}
			else
			{
				var ray = cam.ScreenPointToRay(startSelectionDrag);
				if (Physics.Raycast(ray, out var info, Mathf.Infinity, selectionMask))
				{
					var s = info.collider.GetComponent<ISelectable>();

					if (s != null)
					{
						ChangeSelected(new[] {s}, false);
					}
				}
				else
				{
					ChangeSelected(new ISelectable[0], false);
				}
			}
		}
	}

	private void ChangeSelected(ISelectable[] newSelects, bool dragSelect)
	{
		// deselect old values.
		foreach (var selectable in selectedUnits.Except(newSelects))
		{
			if (selectable != null && selectable.IsSelected)
				selectable.OnDeselect();
		}

		foreach (var selectable in newSelects)
		{
			if (selectable != null && !selectable.IsSelected)
				selectable.OnSelect(dragSelect);
		}

		selectedUnits = newSelects.Where(selectable => selectable != null && selectable.IsSelected).ToArray();
	}

	private static void SetRectTransformFromRect(RectTransform trans, Rect rect)
	{
		trans.anchoredPosition = rect.position;
		trans.sizeDelta = rect.size;
	}

	private static Rect GetRect(Vector2 corner1, Vector2 corner2)
	{
		var width = Mathf.Abs(corner1.x - corner2.x);
		var height = Mathf.Abs(corner1.y - corner2.y);
		var x = Mathf.Min(corner1.x, corner2.x);
		var y = Mathf.Min(corner1.y, corner2.y);
		return new Rect(x, y, width, height);
	}

	private static ISelectable[] GetUnitsInRect(Rect rect, Camera camera)
	{
		float left = rect.xMin;
		float top = rect.yMax;
		float right = rect.xMax;
		float bottom = rect.yMin;

		var bottomLeft = camera.ScreenPointToRay(new Vector3(left, bottom));
		var topLeft = camera.ScreenPointToRay(new Vector3(left, top));
		var topRight = camera.ScreenPointToRay(new Vector3(right, top));
		var bottomRight = camera.ScreenPointToRay(new Vector3(right, bottom));

		var planes = new Plane[5];
		planes[0].Set3Points(topLeft.GetPoint(3), bottomLeft.GetPoint(3), bottomRight.GetPoint(3)); // front
		planes[1].Set3Points(topLeft.origin, topRight.origin, topRight.GetPoint(100)); // top
		planes[2].Set3Points(topLeft.origin, topLeft.GetPoint(100), bottomLeft.GetPoint(100)); //left
		planes[3].Set3Points(bottomLeft.origin, bottomLeft.GetPoint(100), bottomRight.GetPoint(100)); //bottom
		planes[4].Set3Points(topRight.origin, bottomRight.GetPoint(100), topRight.GetPoint(100)); //right

		// TODO: improve performance (replace FindObjectsOfType).
		return FindObjectsOfType<MonoBehaviour>().OfType<ISelectable>()
			.Where(unit => GeometryUtility.TestPlanesAABB(planes, unit.Bounds)).ToArray();
	}
}