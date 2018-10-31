using UnityEngine;
using XInputDotNetPure;


namespace Game.Entity.Player
{
    public class Mover : MonoBehaviour
    {
        [Tooltip("Player mouvement speed")]
        [SerializeField]
        private float speed;

        [Tooltip("Player Jump Speed")]
        [SerializeField]
        private float jumpSpeed;

        [Tooltip("Short period of time where the player can jump while being airborne.")]
        [SerializeField]
        private float playerNoLongerGroundedDelay;

        [Tooltip("Amount of jumps allowed after leaving ground.")]
        [SerializeField]
        private float amountOfAdditionalJumps;


        private PlayerIndex controllerNumber;
        private KinematicRigidbody2D kinematicRigidbody2D;
        private Vector2 direction;
        private GamePadState controllerState;
        private bool jumpButtonPressed;
        private float lastPositionY;
        private float velocityY = 0;
        private Vector2 horizontalVelocity;
        private Vector2 verticalVelocity;
        private int jumpCount;
        private PlayerController player;

        private void Awake()
        {
            kinematicRigidbody2D = GetComponent<KinematicRigidbody2D>();
            direction = Vector2.right;
            controllerNumber = PlayerIndex.One;
            controllerState = GamePad.GetState(controllerNumber);
            jumpButtonPressed = false;
            horizontalVelocity = Vector2.zero;
            verticalVelocity = Vector2.zero;
            jumpCount = 0;
            player = GetComponent<PlayerController>();
        }

        private void Update()
        {
            kinematicRigidbody2D.Velocity = horizontalVelocity * speed + verticalVelocity * jumpSpeed;
            velocityY = this.transform.position.y - lastPositionY;
            velocityY = velocityY / Time.fixedDeltaTime;
            lastPositionY = this.transform.position.y;
            PlayerController.instance.CurrentController.animator.SetFloat(Values.AnimationParameters.Player.VelocityY, velocityY);
            PlayerController.instance.CurrentController.animator.SetFloat(Values.AnimationParameters.Player.Speed, Mathf.Abs(kinematicRigidbody2D.Velocity.x));
            PlayerController.instance.CurrentController.animator.SetBool(Values.AnimationParameters.Player.Grounded, kinematicRigidbody2D.IsGrounded);
            horizontalVelocity = Vector2.zero;
            verticalVelocity = Vector2.zero;
            ResetJumpCount();
        }

        public void Attack()
        {
            PlayerController.instance.CurrentController.UseBasicAttack(PlayerController.instance);
        }

        public void MoveRight()
        {
            horizontalVelocity += Vector2.right;
            direction = Vector2.right;
            player.playerHorizontalDirection = Vector2.right;
            player.DirectionFacingLeftRight = FacingSideLeftRight.Right;
            PlayerController.instance.DirectionFacingUpDown = FacingSideUpDown.None;
           // PlayerController.instance.CurrentController.sprite.flipX = false;
            PlayerController.instance.IsMoving = true;
        }

        public void MoveLeft()
        {
            horizontalVelocity += Vector2.left;
            direction = Vector2.left;
            player.playerHorizontalDirection = Vector2.left;
            PlayerController.instance.DirectionFacingLeftRight = FacingSideLeftRight.Left;
            PlayerController.instance.DirectionFacingUpDown = FacingSideUpDown.None;
           // PlayerController.instance.CurrentController.sprite.flipX = true;
            PlayerController.instance.IsMoving = true;
        }

        public void Jump()
        {
            if (kinematicRigidbody2D.TimeSinceAirborne < playerNoLongerGroundedDelay && jumpCount == 0)
            {
                verticalVelocity = Vector2.up;
                PlayerController.instance.CurrentController.animator.SetTrigger(Values.AnimationParameters.Player.Jump);
                PlayerController.instance.IsMoving = true;
            }
            else if (jumpCount < amountOfAdditionalJumps)
            {
                verticalVelocity = Vector2.up;
                PlayerController.instance.CurrentController.animator.SetTrigger(Values.AnimationParameters.Player.Jump);
                PlayerController.instance.IsMoving = true;
                jumpCount++;
            }
        }

        public void AimUp()
        {
            PlayerController.instance.DirectionFacingUpDown = FacingSideUpDown.Up;
            player.playerVerticalDirection = Vector2.up;
            direction = Vector2.up;
        }

        public void AimDown()
        {
            PlayerController.instance.DirectionFacingUpDown = FacingSideUpDown.Down;
            player.playerVerticalDirection = Vector2.down;
            direction = Vector2.down;
        }

        public void UseCapacity()
        {
            if (PlayerController.instance.CurrentController.CapacityUsable(PlayerController.instance))
                PlayerController.instance.CurrentController.UseCapacity(PlayerController.instance);
        }

        private void ResetJumpCount()
        {
            if (kinematicRigidbody2D.IsGrounded)
            {
                jumpCount = 0;
            }
        }
    }
}
