using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Camera cam;

	[Range(0, 0.5f)] public float edgeDistance = 0.05f;

	public Vector2 minSpeed;
	public Vector2 maxSpeed;

	public float scrollSpeed;

	public float speedOverHeight;

	public Vector3 min, max;

	public bool disableWhenMouseIsOutsideWindow = true;

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


		transform.position = pos;

		transform.position = ClampToScreenPoint(transform.position);
	}

	private Vector3 ClampToScreenPoint(Vector3 position)
	{
		var pixelWidth = cam.pixelWidth;
		var pixelHeight = cam.pixelHeight;

		var center = cam.ScreenPointToRay(new Vector3(pixelWidth / 2f, pixelHeight / 2f));
		// create a plane at 0,0,0 whose normal points to +Y:
		Plane hPlane = new Plane(Vector3.up, Vector3.zero);
		// Plane.Raycast stores the distance from ray.origin to the hit point in this variable:
		// if the ray hits the plane...
		if (!hPlane.Raycast(center, out var distance)) return position;
		// get the hit point:
		var point = center.GetPoint(distance);

		var clampPoint = new Vector3(Mathf.Clamp(point.x, min.x, max.x), 0, Mathf.Clamp(point.z, min.z, max.z));

		if (point == clampPoint) return position;
		var diff = point - clampPoint;

		position -= diff;

		return position;

		// var top = cam.ScreenPointToRay(new Vector3(pixelWidth / 2f, pixelHeight * screenCornerOffsetMax.y));
		// var bottom = cam.ScreenPointToRay(new Vector3(pixelWidth / 2f, pixelHeight * screenCornerOffsetMin.y));
		// var right = cam.ScreenPointToRay(new Vector3(pixelWidth * screenCornerOffsetMax.x, pixelHeight / 2f));
		// var left = cam.ScreenPointToRay(new Vector3(pixelWidth * screenCornerOffsetMin.x, pixelHeight / 2f));
		//
		// if (Physics.Raycast(top, Mathf.Infinity, layerMask: excludeNonVisible))
		// {
		// 	if (movement.x > 0)
		// 		position.x += movement.x;
		// }
		//
		// if (Physics.Raycast(bottom, Mathf.Infinity, layerMask: excludeNonVisible))
		// {
		// 	if (movement.x < 0)
		// 		position.x += movement.x;
		// }
		//
		// if (Physics.Raycast(right, Mathf.Infinity, layerMask: excludeNonVisible))
		// {
		// 	if (movement.z > 0)
		// 		position.z += movement.z;
		// }
		//
		// if (Physics.Raycast(left, Mathf.Infinity, layerMask: excludeNonVisible))
		// {
		// 	if (movement.z < 0)
		// 		position.z += movement.z;
		// }
	}

	private static bool ClampVector2(ref Vector2 val, Vector2 min, Vector2 max)
	{
		var oldVal = val;
		val = new Vector2(Mathf.Clamp(val.x, min.x, max.x), Mathf.Clamp(val.y, min.y, max.y));

		return oldVal != val;
	}

	private static bool ClampVector3(ref Vector3 val, Vector3 min, Vector3 max)
	{
		var oldVal = val;
		val = new Vector3(Mathf.Clamp(val.x, min.x, max.x),
			Mathf.Clamp(val.y, min.y, max.y),
			Mathf.Clamp(val.z, min.z, max.z));

		return oldVal != val;
	}
}