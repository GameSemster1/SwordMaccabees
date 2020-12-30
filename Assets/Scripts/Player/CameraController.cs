using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class that controls the main camera.
/// </summary>
public class CameraController : MonoBehaviour
{
	[SerializeField] [Tooltip("The main camera.")]
	private Camera cam;

	[SerializeField]
	[Range(0, 0.5f)]
	[Tooltip(
		"The distance from the edge the mouse has to be for the camera to start moving, as a fraction of the screen size.")]
	private float edgeDistance = 0.05f;

	[SerializeField]
	[Tooltip("The minimum movement speed of the camera (when the mouse is exactly at 'Edge Distance').")]
	private Vector2 minSpeed;

	[SerializeField]
	[Tooltip("The maximum movement speed of the camera (when the mouse is exactly at the edge of the screen).")]
	private Vector2 maxSpeed;

	[SerializeField] [Tooltip("The camera's scrolling speed (z-axis movement)")]
	private float scrollSpeed;

	[SerializeField] [Tooltip("How much should we scale the movement speed by the height?")]
	private float speedOverHeight;

	[SerializeField] [Tooltip("The lower limits of the camera's position.")]
	private Vector3 min;

	[SerializeField] [Tooltip("The upper limits of the camera's position.")]
	private Vector3 max;

	[SerializeField] [Tooltip("Should we disable the camera's movement when the mouse is outside the game window?")]
	private bool disableWhenMouseIsOutsideWindow = true;

	// Update is called once per frame
	private void Update()
	{
		var pos = transform.position;
		var mousePos = new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);

		if (ClampVector2(ref mousePos, Vector2.zero, Vector2.one) && disableWhenMouseIsOutsideWindow)
		{
			return;
		}

		var movement = Vector3.zero;

		if (mousePos.x <= edgeDistance)
		{
			// -x
			movement.x -= minSpeed.x + (maxSpeed.x - minSpeed.x) * (1 - mousePos.x / edgeDistance);
		}

		if (mousePos.x >= 1 - edgeDistance)
		{
			// +x
			movement.x += minSpeed.x +
			              (maxSpeed.x - minSpeed.x) * (1 - (1 - mousePos.x) / edgeDistance);
		}

		if (mousePos.y <= edgeDistance)
		{
			// -z
			movement.z -= minSpeed.y + (maxSpeed.y - minSpeed.y) * (1 - mousePos.y / edgeDistance);
		}

		if (mousePos.y >= 1 - edgeDistance)
		{
			// +z
			movement.z += minSpeed.y +
			              (maxSpeed.y - minSpeed.y) * (1 - (1 - mousePos.y) / edgeDistance);
		}

		pos.y += Input.mouseScrollDelta.y * scrollSpeed;
		pos.y = Mathf.Clamp(pos.y, min.y, max.y);

		pos += movement * (speedOverHeight * pos.y * Time.deltaTime);

		pos -= ScreenPointLimitDifference(min, max, cam);

		transform.position = pos;
	}

	/// <summary>
	/// Clamps the intersection of the center of the screen's ray with the y=0 plane by 'min' and 'max'.
	/// </summary>
	/// <returns>The difference that will place the point inside the limits.</returns>
	private static Vector3 ScreenPointLimitDifference(Vector3 min, Vector3 max, Camera cam)
	{
		var pixelWidth = cam.pixelWidth;
		var pixelHeight = cam.pixelHeight;

		var center = cam.ScreenPointToRay(new Vector3(pixelWidth / 2f, pixelHeight / 2f));
		// create a plane at 0,0,0 whose normal points to +Y:
		Plane hPlane = new Plane(Vector3.up, Vector3.zero);
		// Plane.Raycast stores the distance from ray.origin to the hit point in this variable:
		// if the ray hits the plane...
		if (!hPlane.Raycast(center, out var distance)) return Vector3.zero;
		// get the hit point:
		var point = center.GetPoint(distance);

		var clampPoint = new Vector3(Mathf.Clamp(point.x, min.x, max.x), 0, Mathf.Clamp(point.z, min.z, max.z));

		if (point == clampPoint) return Vector3.zero;
		var diff = point - clampPoint;
		return diff;
	}

	/// <summary>
	/// Clamps 'val' by 'min' and 'max'.
	/// </summary>
	/// <returns>True if val was changed.</returns>
	private static bool ClampVector2(ref Vector2 val, Vector2 min, Vector2 max)
	{
		var oldVal = val;
		val = new Vector2(Mathf.Clamp(val.x, min.x, max.x), Mathf.Clamp(val.y, min.y, max.y));

		return oldVal != val;
	}

	/// <summary>
	/// Clamps 'val' by 'min' and 'max'.
	/// </summary>
	/// <returns>True if val was changed.</returns>
	private static bool ClampVector3(ref Vector3 val, Vector3 min, Vector3 max)
	{
		var oldVal = val;
		val = new Vector3(Mathf.Clamp(val.x, min.x, max.x),
			Mathf.Clamp(val.y, min.y, max.y),
			Mathf.Clamp(val.z, min.z, max.z));

		return oldVal != val;
	}
}