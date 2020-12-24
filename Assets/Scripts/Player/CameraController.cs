using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Camera cam;

	[Range(0, 1)] public float edgeDistance = 0.05f;

	public Vector2 minSpeed;
	public Vector2 maxSpeed;

	public float scrollSpeed;

	public float speedOverHeight;

	public float minHeight, maxHeight;

	private float height;

	// Start is called before the first frame update
	void Start()
	{
		height = transform.position.y;
	}

	// Update is called once per frame
	void Update()
	{
		var mousePos = new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);

		mousePos = ClampVector2(mousePos, Vector2.zero, Vector2.one);

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

		height += Input.mouseScrollDelta.y * scrollSpeed;
		height = Mathf.Clamp(height, minHeight, maxHeight);

		transform.Translate(movement * (speedOverHeight * height * Time.deltaTime));

		SetHeight(transform, height);
	}

	private static void SetHeight(Transform trans, float height)
	{
		var position = trans.position;
		position = new Vector3(position.x, height, position.z);
		trans.position = position;
	}

	private static Vector2 ClampVector2(Vector2 val, Vector2 min, Vector2 max)
	{
		return new Vector2(Mathf.Clamp(val.x, min.x, max.x), Mathf.Clamp(val.y, min.y, max.y));
	}
}