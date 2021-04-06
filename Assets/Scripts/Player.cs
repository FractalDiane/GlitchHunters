using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	SpriteRenderer sprite;
	new Rigidbody rigidbody;
	AudioSource spinSound;
	Animator animator;

	const float JumpForce = 20.0f;

	float horizontal = 0f;
	float vertical = 0f;

	bool onGround = false;
	bool canSpin = true;
	bool spunRecently = false;
	bool jumpedRecently = false;

	[SerializeField]
	LayerMask collisionMask;

	[SerializeField]
	ParticleSystem trickParticles = null;

	[SerializeField]
	float speed = 10f;

	Vector3 addedVelocity = Vector3.zero;
	public Vector3 AddedVelocity { get => addedVelocity; set => addedVelocity = value; }
	bool onPlatform = false;
	public bool OnPlatform { get => onPlatform; set => onPlatform = value; }

	bool lockMovement = false;
	public bool LockMovement { get => lockMovement; set => lockMovement = value; }
	bool lockCamera = false;
	public bool LockCamera { get => lockCamera; set => lockCamera = value; }
	

	void Start()
	{
		rigidbody = GetComponent<Rigidbody>();
		sprite = GetComponentInChildren<SpriteRenderer>();
		spinSound = GetComponent<AudioSource>();
		animator = GetComponent<Animator>();

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
		if (onGround)
		{
			canSpin = true;
			if (!onPlatform)
			{
				addedVelocity = Vector3.zero;
			}

			if (!lockMovement && Input.GetButtonDown("Jump"))
			{
				rigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
				jumpedRecently = true;
				Invoke(nameof(UnsetJumpedRecently), 0.5f);
			}
		}

		// Shorter jump if it isn't held down as long
		if (rigidbody.velocity.y > 0f && (!Input.GetButton("Jump") || lockMovement))
		{
			Vector3 vel = rigidbody.velocity;
			vel.y -= 100f * Time.deltaTime;
			rigidbody.velocity = vel;
		}

		if (canSpin && !lockMovement && Input.GetButtonDown("Fire1"))
		{
			spinSound.Play();
			animator.Play("Spin");
			trickParticles.Play();
			Vector3 vel = rigidbody.velocity;
			vel.y = 0f;
			rigidbody.velocity = vel;
			rigidbody.AddForce(new Vector3(0, 18f, 0), ForceMode.Impulse);
			canSpin = false;

			spunRecently = true;
			Invoke(nameof(UnsetSpunRecently), 0.5f);
		}

		Quaternion currentRot = sprite.transform.rotation;
		Transform xform = sprite.transform;
		xform.LookAt(Camera.main.transform.position, Vector3.up);
		Quaternion newRot = xform.rotation;
		sprite.transform.rotation = Quaternion.Slerp(currentRot, newRot, 0.01f);

		// *** GLITCH DETECTION: MEGA JUMP ***
		if (jumpedRecently && spunRecently && rigidbody.velocity.y >= 14f)
		{
			GlitchProgress.Singleton.CompleteGlitch("megajump");
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
		rigidbody.velocity = result + AddedVelocity;
	}

	void UnsetSpunRecently()
	{
		spunRecently = false;
	}

	void UnsetJumpedRecently()
	{
		jumpedRecently = false;
	}
}
