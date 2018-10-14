using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class PlatformMover : MonoBehaviour
{

	[Tooltip("Distance traveled by the platform when heading left.(From start to this)")]
	[SerializeField] private float MaxDistanceLeft;
	[Tooltip("Distance traveled by the platform when heading right.(From start to this this)")]
	[SerializeField] private float MaxDistanceRight;
	[Tooltip("Distance Travelled by the platform when heading upwards.(From start to this")]
	[SerializeField] private float MaxDistanceUp;
	[Tooltip("Distance traveled by the platform when heading downwards.(From start to this")]
	[SerializeField] private float MaxDistanceDown;
	[Tooltip("Speed at which the platform moves horizontaly.")]
	[SerializeField] private float HorizontalSpeed;
	[Tooltip("Speed at which the platform moves verticaly.")]
	[SerializeField] private float VerticalSpeed;
	[Tooltip("True when heading towards the right. (Checking this will make the platform head towards the right first.")]
	[SerializeField] private bool isHeadingRight;
	[Tooltip("True when heading up. (Checking this will make the platform head upwards first.")]
	[SerializeField] private bool isHeadingUpwards;

	private float initialPositionX;
	private float initialPositionY;
	private Rigidbody2D rb;
	private Vector2 horizontalDirection;
	private Vector2 verticalDirection;
	
	
	// Use this for initialization
	void Start ()
	{
		rb = GetComponent<Rigidbody2D>();
		initialPositionX = rb.position.x;
		initialPositionY = rb.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		
		CheckHorizontalDirection();
		checkVertialDirection();
		//rb.velocity.Set(horizontalDirection.x*HorizontalSpeed ,verticalDirection.y*VerticalSpeed);
		//rb.velocity = new Vector2(horizontalDirection.x * HorizontalSpeed, verticalDirection.y * VerticalSpeed);
	}
	
	

	private void FixedUpdate()
	{
		rb.velocity = new Vector2(horizontalDirection.x * HorizontalSpeed, verticalDirection.y * VerticalSpeed);
	}

	private void OnCollisionStay2D(Collision2D other)
	{
		if (other.collider.CompareTag("Player"))
		{
			other.transform.parent = gameObject.transform;
		}
	}

	private void OnCollisionExit2D(Collision2D other)
	{
		if (other.collider.CompareTag("Player"))
		{
			other.transform.parent = null;
		}
	}

	private void CheckHorizontalDirection()
	{
		if (isHeadingRight)
		{
			if (transform.position.x < initialPositionX + MaxDistanceRight)
			{
				horizontalDirection = Vector2.right;
			}
			else
			{
				isHeadingRight = false;
			}
			
		}
		else
		{
			if (transform.position.x > initialPositionX - MaxDistanceLeft)
			{
				horizontalDirection = Vector2.left;
			}
			else
			{
				isHeadingRight = true;
			}
		}
	}

	private void checkVertialDirection()
	{
		if (isHeadingUpwards)
		{
			if (transform.position.y < initialPositionY + MaxDistanceUp)
			{
				verticalDirection = Vector2.up;
			}
			else
			{
				isHeadingUpwards = false;
			}
			
		}
		else
		{
			if (transform.position.y > initialPositionY - MaxDistanceDown)
			{
				verticalDirection = Vector2.down;
			}
			else
			{
				isHeadingUpwards = true;
			}
		}
	}
	
	
}
