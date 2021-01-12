using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;

/// <summary>
/// A class that is in charge of selecting units.
/// </summary>
public class UnitSelector : MonoBehaviour
{
	[SerializeField] [Tooltip("The main camera.")]
	private Camera cam;

	[SerializeField] [Tooltip("The mouse button (0, 1, or 2) for selecting.")]
	private int mouseButton;

	/// <summary>
	/// Should be an image (preferably a 9-split) who's anchor is the bottom left corner.
	/// </summary>
	[SerializeField] [Tooltip("A UI element that shows the selection rectangle.")]
	private RectTransform selectionTransform;

	[SerializeField]
	[Tooltip(
		"A mask of the objects that can be selected. Objects must also have a component that implements 'ISelectable'.")]
	private LayerMask selectionMask;

	private Vector2 startSelectionDrag, endSelectionDrag;
	private Rect selectionBox;

	private ISelectable[] selectedUnits = new ISelectable[0];

	/// <summary>
	/// All the units that are currently selected.
	/// </summary>
	public IEnumerable<ISelectable> SelectedUnits =>
		selectedUnits.Where(selectable => (Component) selectable != null && selectable.IsSelected);

	/// <summary>
	/// The unit that is currently highlighted. Null if there is none.
	/// </summary>
	public ISelectable HighlightedUnit { get; private set; }

	private bool isDragging = false;

	private bool isSelecting = false;

	private void Update()
	{
		if (Input.GetMouseButtonDown(mouseButton) && !IsPointerOverUIObject())
		{
			// start selecting
			startSelectionDrag = Input.mousePosition;
			isSelecting = true;
		}

		if (Input.GetMouseButton(mouseButton) && isSelecting)
		{
			Vector2 currentSelectionDrag = Input.mousePosition;

			if (isDragging || currentSelectionDrag != startSelectionDrag)
			{
				// The mouse is being dragged!
				isDragging = true;
				selectionTransform.gameObject.SetActive(true);
				selectionBox = GetRect(startSelectionDrag, currentSelectionDrag);

				SetRectTransformFromRect(selectionTransform, selectionBox);
			}
		}
		else // The mouse button is not pressed. Check for highlighting.
		{
			var ray = cam.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out var info, Mathf.Infinity, selectionMask))
			{
				var s = info.collider.GetComponent<ISelectable>();

				if ((MonoBehaviour) s != null)
				{
					if (HighlightedUnit != s)
					{
						if ((MonoBehaviour) HighlightedUnit != null)
							HighlightedUnit.OnHighlight(false);

						s.OnHighlight(true);
						HighlightedUnit = s;
					}
				}
				else
				{
					if ((MonoBehaviour) HighlightedUnit != null)
						HighlightedUnit.OnHighlight(false);
					HighlightedUnit = null;
				}
			}
			else
			{
				if ((MonoBehaviour) HighlightedUnit != null)
					HighlightedUnit.OnHighlight(false);
				HighlightedUnit = null;
			}
		}

		if (Input.GetMouseButtonUp(mouseButton) && isSelecting)
		{
			isSelecting = false;
			if (isDragging)
			{
				// The mouse moved. Select all units the rectangle.
				isDragging = false;

				selectionTransform.gameObject.SetActive(false);
				endSelectionDrag = Input.mousePosition;
				selectionBox = GetRect(startSelectionDrag, endSelectionDrag);
				ChangeSelected(GetUnitsInRect(selectionBox, cam), true);
			}
			else
			{
				// The mouse didn't move. select the unit at the mouse's position.
				var ray = cam.ScreenPointToRay(startSelectionDrag);
				if (Physics.Raycast(ray, out var info, Mathf.Infinity, selectionMask))
				{
					var s = info.collider.GetComponent<ISelectable>();

					if ((MonoBehaviour) s != null)
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

	private static bool IsPointerOverUIObject()
	{
		PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current)
		{
			position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
		};
		var results = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
		return results.Count > 0;
	}

	private void ChangeSelected(ISelectable[] newSelects, bool dragSelect)
	{
		// deselect old values.
		foreach (var selectable in selectedUnits.Except(newSelects))
		{
			if ((MonoBehaviour) selectable != null && selectable.IsSelected)
				selectable.OnDeselect();
		}

		// select new values.
		foreach (var selectable in newSelects)
		{
			if ((MonoBehaviour) selectable != null && !selectable.IsSelected)
				selectable.OnSelect(dragSelect);
		}

		// update selection.
		selectedUnits = newSelects.Where(selectable => (MonoBehaviour) selectable != null && selectable.IsSelected)
			.ToArray();
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

	/// <summary>
	/// Based on: https://gist.github.com/benloong/4661195
	/// </summary>
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