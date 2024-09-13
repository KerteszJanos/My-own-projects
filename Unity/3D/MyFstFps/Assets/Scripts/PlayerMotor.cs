using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
	private CharacterController controller;
	private Vector3 playerVelocity;
	public float speed = 5f;
	bool isGrounded;
	public float gravity = -9.8f;


	public float jumpHight = 1f;

	private bool lerpCrouch;
	private float crouchTimer;
	private bool crouching;

	private bool sprinting;
	// Start is called before the first frame update
	void Start()
	{
		controller = GetComponent<CharacterController>();
		lerpCrouch = true;
		crouchTimer = 0f;
		crouching = false;

		sprinting = false;
	}

	// Update is called once per frame
	void Update()
	{
		isGrounded = controller.isGrounded;

		if (lerpCrouch)
		{
			crouchTimer += Time.deltaTime;
			float p = crouchTimer / 1;
			p *= p;

			if (crouching)
			{
				controller.height = Mathf.Lerp(controller.height, 1, p);
				speed = 3;
			}
			else
			{
				controller.height = Mathf.Lerp(controller.height, 2, p);
				speed = 5;
			}

			if (p > 1)
			{
				lerpCrouch = false;
				crouchTimer = 0f;
			}
		}
	}

	//recieve the imputs from our InputManager.cs and apply them to our character controller.
	public void ProcessMove(Vector2 input)
	{
		Vector3 moveDirection = Vector3.zero;
		moveDirection.x = input.x;
		moveDirection.z = input.y;
		controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);

		playerVelocity.y += gravity * Time.deltaTime;
		if (isGrounded && playerVelocity.y < 0)
		{
			playerVelocity.y = -2f;
		}
		controller.Move(playerVelocity * Time.deltaTime);

		Debug.Log(playerVelocity.y);
	}

	public void Jump()
	{
		if (isGrounded && !crouching)
		{
			playerVelocity.y = Mathf.Sqrt(jumpHight * -3.0f * gravity);
		}
	}

	public void Crouch()
	{
		crouching = !crouching;
		crouchTimer = 0;
		lerpCrouch = true;
	}

	public void Sprint()
	{
		if (crouching)
		{
			return;
		}
		speed = 8;
	}

	public void SprintEnded()
	{
		if (!crouching)
		{
			speed = 5;
		}
	}
}
