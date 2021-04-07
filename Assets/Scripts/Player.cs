using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	SpriteRenderer sprite;
	new Rigidbody rigidbody;
	AudioSource spinSound;
	AudioSource jumpSound;
	AudioSource landSound;
	Animator animator;

	const float JumpForce = 20.0f;

	float horizontal = 0f;
	float vertical = 0f;

	bool onGround = false;
	bool canSpin = true;
	bool spunRecently = false;
	bool spinWaitForNotGrounded = false;
	bool jumpedRecently = false;
	bool jumpWaitForNotGrounded = false;

	[SerializeField]
	bool trackGlitches = true;

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
	
	public float jumpStartHeight;
	public float jumpHeight;

	void Start()
	{
		rigidbody = GetComponent<Rigidbody>();
		sprite = GetComponentInChildren<SpriteRenderer>();
		animator = GetComponent<Animator>();
		AudioSource[] sources = GetComponents<AudioSource>();
		spinSound = sources[0];
		jumpSound = sources[1];
		landSound = sources[2];

		Cursor.lockState = CursorLockMode.Locked;
	}

	void Update()
	{
		bool spinning = IsSpinning();

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
		bool landed = Physics.SphereCast(transform.position, 0.5f, Vector3.down, out hit, 1.2f, collisionMask);
		if (!onGround && landed)
		{
			landSound.Play();
		}
		onGround = landed;
		//bool jumpedOnThisFrame = false;
		if (onGround)
		{
			canSpin = true;
			if(!spinWaitForNotGrounded)
				spunRecently = false;
			if (!onPlatform)
			{
				addedVelocity = Vector3.zero;
			}

			if (!lockMovement && Input.GetButtonDown("Jump"))
			{
				jumpSound.Play();
				rigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
				jumpedRecently = true;
				Invoke(nameof(UnsetJumpedRecently), 0.5f);
				//jumpedOnThisFrame = true;
				jumpWaitForNotGrounded = true;
				// *** GLITCH DETECTION: MEGA JUMP ***
				if (trackGlitches && spinning&&rigidbody.velocity.y>=0)
				{
					// GlitchProgress.Singleton.CompleteGlitch("megajump");
					spunRecently = true;
				}
			}
			jumpStartHeight = transform.position.y;
			jumpHeight = 0;
		} else {
			float currentJumpHeight = transform.position.y-jumpStartHeight;
			// float currentJumpHeight = rigidbody.velocity.y;
			if(currentJumpHeight > jumpHeight) {
				jumpHeight = currentJumpHeight;
				PlayerUI.Singleton.DisplayJumpHeight(jumpHeight);
			}
			if(spinWaitForNotGrounded) {
				spinWaitForNotGrounded = false;
			}
			jumpWaitForNotGrounded = false;
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
			if(trackGlitches && spinning)
			{
				GlitchProgress.Singleton.CompleteGlitch("double_spin");
			}
			spinSound.Play();
			animator.Play("Spin");
			trickParticles.Play();
			Vector3 vel = rigidbody.velocity;
			vel.y = 0f;
			rigidbody.velocity = vel;
			rigidbody.AddForce(new Vector3(0, 18f, 0), ForceMode.Impulse);
			canSpin = false;

			spunRecently = true;
			spinWaitForNotGrounded = true;
			//Invoke(nameof(UnsetSpunRecently), 0.5f);
		}

		Quaternion currentRot = sprite.transform.rotation;
		Transform xform = sprite.transform;
		xform.LookAt(Camera.main.transform.position, Vector3.up);
		Quaternion newRot = xform.rotation;
		sprite.transform.rotation = Quaternion.Slerp(currentRot, newRot, 0.01f);

		// *** GLITCH DETECTION: slope jump ***
		if (trackGlitches && jumpHeight >= 5f && !spunRecently && !spinning)
		{
			GlitchProgress.Singleton.CompleteGlitch("jump_5");
		}

		

		// // *** GLITCH DETECTION: MEGA JUMP ***
		if (trackGlitches && rigidbody.velocity.y>=30f&&spinning)
		{
			GlitchProgress.Singleton.CompleteGlitch("megajump");
		}

		// *** GLITCH DETECTION: DOUBLE MEGA JUMP 20 DELUXE ***
		if (trackGlitches && jumpHeight >= 20f)
		{
			GlitchProgress.Singleton.CompleteGlitch("jump_20");
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

	void UnsetJumpedRecently()
	{
		jumpedRecently = false;
	}

	bool IsSpinning() {
		return animator.GetCurrentAnimatorStateInfo(0).IsName("Spin");
	}
}
