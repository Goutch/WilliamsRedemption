
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;


	public class Mover : MonoBehaviour
	{
		[Tooltip("Player mouvement speed")]
		[SerializeField] private float speed;
		[Tooltip("Player Jump Speed")]
		[SerializeField] private float jumpSpeed;
		[Tooltip("Short period of time where the player can jump while being airborne.")]
		[SerializeField] private float playerNoLongerGroundedDelay;
		[Tooltip("Amount of jumps allowed after leaving ground.")] [SerializeField]
		private float amountOfAdditionalJumps;


		private PlayerIndex controllerNumber;
		private KinematicRigidbody2D kinematicRigidbody2D;
		private Vector2 direction;
		private PlayerController player;
		private GamePadState controllerState;
		private bool jumpButtonPressed;
		private float lastPositionY;
		private float velocityY = 0;
		private Vector2 horizontalVelocity;
		private Vector2 verticalVelocity;
		private int jumpCount;
		
		private void Awake()
		{
			kinematicRigidbody2D = GetComponent<KinematicRigidbody2D>();
			direction = Vector2.right;
			player = GetComponent<PlayerController>();
			controllerNumber = PlayerIndex.One;
			controllerState = GamePad.GetState(controllerNumber);
			jumpButtonPressed = false;
			horizontalVelocity = Vector2.zero;
			verticalVelocity = Vector2.zero;
			jumpCount = 0;
		}

		private void Update()
		{
			
			kinematicRigidbody2D.Velocity = horizontalVelocity * speed +verticalVelocity * jumpSpeed;
			velocityY = this.transform.position.y - lastPositionY;
			velocityY = velocityY / Time.fixedDeltaTime;
			lastPositionY = this.transform.position.y;
			player.CurrentController.animator.SetFloat("VelocityY", velocityY);
			player.CurrentController.animator.SetFloat("Speed", Mathf.Abs(kinematicRigidbody2D.Velocity.x));
			player.CurrentController.animator.SetBool("Grounded",kinematicRigidbody2D.IsGrounded);
			horizontalVelocity = Vector2.zero;
			verticalVelocity = Vector2.zero;
			ResetJumpCount();	
		}

		public void Attack()
		{
			player.CurrentController.UseBasicAttack(player, direction);
		}

		public void MoveRight()
		{	
			horizontalVelocity += Vector2.right;
			direction = Vector2.right;
			player.DirectionFacingLeftRight = FacingSideLeftRight.Right;
			player.DirectionFacingUpDown = FacingSideUpDown.None;
			player.CurrentController.sprite.flipX = false;
			player.IsMoving = true;
			
		}

		public void MoveLeft()
		{
			horizontalVelocity += Vector2.left;
			direction = Vector2.left;
			player.DirectionFacingLeftRight = FacingSideLeftRight.Left;
			player.DirectionFacingUpDown = FacingSideUpDown.None;
			player.CurrentController.sprite.flipX = true;
			player.IsMoving = true;
		}

		public void Jump()
		{
			if (kinematicRigidbody2D.TimeSinceAirborne < playerNoLongerGroundedDelay && jumpCount ==0)
			{
				verticalVelocity = Vector2.up;
				player.CurrentController.animator.SetTrigger("Jump");
				player.IsMoving = true;
			}
			else if (jumpCount<amountOfAdditionalJumps)
			{
				verticalVelocity = Vector2.up;
				player.CurrentController.animator.SetTrigger("Jump");
				player.IsMoving = true;
				jumpCount++;
			}	
		}

		public void AimUp()
		{
			player.DirectionFacingUpDown = FacingSideUpDown.Up;
			direction = Vector2.up;
		}

		public void AimDown()
		{
			player.DirectionFacingUpDown = FacingSideUpDown.Down;
			direction = Vector2.down;
		}

		public void UseCapacity()
		{
			if (player.CurrentController.CapacityUsable(player))
				player.CurrentController.UseCapacity(player, direction);
		}

		private void ResetJumpCount()
		{
			if (kinematicRigidbody2D.IsGrounded )
			{
				jumpCount = 0;
			}
		}
	
	}

