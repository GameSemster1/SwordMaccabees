using UnityEngine;

/// <summary>
/// An interface of the objects that can be selected.
/// </summary>
public interface ISelectable
{
	/// <summary>
	/// The bounds of this object. Usually Collider.bounds or Renderer.bounds.
	/// </summary>
	Bounds Bounds { get; }

	/// <summary>
	/// Is this object currently selected?
	/// Should be set in OnSelect and OnDeselect.
	/// </summary>
	bool IsSelected { get; }

	/// <summary>
	/// Called on the frame this object is highlighted with 'isHighlighted=true'
	/// or on the frame this object stops being highlighted with 'isHighlighted=false'. 
	/// </summary>
	/// <param name="isHighlighted"> Is this object highlighted?</param>
	void OnHighlight(bool isHighlighted);

	/// <summary>
	/// Called when this object is selected.
	/// You should set IsSelected to true here.
	/// </summary>
	/// <param name="dragSelect">Was this object selected using drag selection?</param>
	void OnSelect(bool dragSelect);

	/// <summary>
	/// Called when this object is deselected.
	/// You should set IsSelected to false here.
	/// </summary>
	void OnDeselect();

	/// <summary>
	/// Called when an action is registered for this object.
	/// </summary>
	/// <param name="position">The position of the action.</param>
	/// <param name="obj">The object at 'position'</param>
	void ActionAt(Vector3 position, GameObject obj);
}