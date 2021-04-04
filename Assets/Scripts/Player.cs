using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public struct Glitch
	{
		string longText;
		bool completed;
	}

	SpriteRenderer sprite;
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
	GameObject glitchListPrefab = null;

	[SerializeField]
	float speed = 10f;

	bool lockMovement = false;
	public bool LockMovement { get => lockMovement; set => lockMovement = value; }

	void Start()
	{
		rigidbody = GetComponent<Rigidbody>();
		sprite = GetComponentInChildren<SpriteRenderer>();

		Cursor.lockState = CursorLockMode.Locked;
	}

	void Update()
	{
		if (!lockMovement) 
		{
			horizontal = Input.GetAxisRaw("Horizontal");
			vertical = Input.GetAxisRaw("Vertical");
		}
		else
		{
			horizontal = 0f;
			vertical = 0f;
		}

		RaycastHit hit;
		onGround = Physics.SphereCast(transform.position, 0.5f, Vector3.down, out hit, 1.2f, collisionMask);

		if (onGround && !lockMovement && Input.GetButtonDown("Jump"))
		{
			rigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
		}

		if (rigidbody.velocity.y > 0f && (!Input.GetButton("Jump") || lockMovement))
		{
			Vector3 vel = rigidbody.velocity;
			vel.y -= 100f * Time.deltaTime;
			rigidbody.velocity = vel;
		}

		Quaternion currentRot = sprite.transform.rotation;
		Transform xform = sprite.transform;
		xform.LookAt(Camera.main.transform.position, Vector3.up);
		Quaternion newRot = xform.rotation;
		sprite.transform.rotation = Quaternion.Slerp(currentRot, newRot, 0.01f);

		if (!lockMovement && Input.GetButtonDown("Pause"))
		{
			lockMovement = true;
			var menu = Instantiate(glitchListPrefab, Vector3.zero, Quaternion.identity);
			menu.GetComponent<GlitchList>().ShowList(new Dictionary<string, bool>(){
				{"Do a thing", false},
				{"Do another thing", false},
				{"Thing you've already done", true},
			});
		}
	}

	void FixedUpdate()
	{
		Vector3 target = new Vector3(horizontal, 0f, vertical);
		target =  Camera.main.transform.TransformDirection(target);
		if (horizontal != 0f || vertical != 0f)
		{
			target.Normalize();
		}

		Vector3 result = target * speed;
		result.y = rigidbody.velocity.y;
		rigidbody.velocity = result;
	}
}
