using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	SpriteRenderer sprite;
	Camera playerCamera;
	new Rigidbody rigidbody;

	const float JumpForce = 20.0f;

	float horizontal = 0f;
	float vertical = 0f;

	bool onGround = false;
	[SerializeField]
	LayerMask collisionMask;

	[SerializeField]
	GameObject cameraPivot = null;

	[SerializeField]
	float speed = 10f;

	void Start()
	{
		rigidbody = GetComponent<Rigidbody>();
		sprite = GetComponentInChildren<SpriteRenderer>();
		playerCamera = GetComponentInChildren<Camera>();

		Cursor.lockState = CursorLockMode.Locked;
	}

	void Update()
	{
		horizontal = Input.GetAxisRaw("Horizontal");
		vertical = Input.GetAxisRaw("Vertical");

		RaycastHit hit;
		onGround = Physics.SphereCast(transform.position, 0.5f, Vector3.down, out hit, 1.2f, collisionMask);

		if (onGround && Input.GetButtonDown("Jump"))
		{
			rigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
		}

		if (rigidbody.velocity.y > 0f && !Input.GetButton("Jump"))
		{
			Vector3 vel = rigidbody.velocity;
			vel.y -= 100f * Time.deltaTime;
			rigidbody.velocity = vel;
		}

		Quaternion currentRot = sprite.transform.rotation;
		Transform xform = sprite.transform;
		xform.LookAt(playerCamera.transform.position, Vector3.up);
		Quaternion newRot = xform.rotation;
		sprite.transform.rotation = Quaternion.Slerp(currentRot, newRot, 0.01f);
	}

	void FixedUpdate()
	{
		Vector3 target = new Vector3(horizontal, 0f, vertical);
		target =  playerCamera.transform.TransformDirection(target);
		if (horizontal != 0f || vertical != 0f)
		{
			target.Normalize();
		}

		Vector3 result = target * speed;
		result.y = rigidbody.velocity.y;
		rigidbody.velocity = result;
	}
}
