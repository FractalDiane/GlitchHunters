using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
	Vector2 mouseInput;

	void Update()
	{
		mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
	}

	void FixedUpdate()
	{
		transform.RotateAround(transform.position, new Vector3(0, 1, 0), mouseInput.x * 10f);
	}
}
