using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
	[SerializeField]
	float speed = 1f;
	[SerializeField]
	Vector3 direction;
	[SerializeField]
	float movementInterval = 1f;

	new Rigidbody rigidbody;

	Player player = null;

	void Start()
	{
		rigidbody = GetComponent<Rigidbody>();
		rigidbody.velocity = direction * speed;
		InvokeRepeating(nameof(SwapDirection), movementInterval, movementInterval);
	}

	void OnCollisionEnter(Collision collision)
	{
		player = collision.gameObject.GetComponent<Player>();
		player.OnPlatform = true;
	}

	void OnCollisionExit(Collision collision)
	{
		player.OnPlatform = false;
		player = null;
	}

	void FixedUpdate()
	{
		if (player != null)
		{
			player.AddedVelocity = rigidbody.velocity;
		}
	}
	
	void SwapDirection()
	{
		direction *= -1;
		rigidbody.velocity = direction * speed;
	}
}
