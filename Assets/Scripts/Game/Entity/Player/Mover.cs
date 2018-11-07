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
            //kinematicRigidbody2D.Velocity = horizontalVelocity * speed + verticalVelocity * jumpSpeed;
            velocityY = this.transform.position.y - lastPositionY;
            velocityY = velocityY / Time.fixedDeltaTime;
            lastPositionY = this.transform.position.y;
            player.CurrentController.animator.SetFloat(Values.AnimationParameters.Player.VelocityY, velocityY);
            player.CurrentController.animator.SetFloat(Values.AnimationParameters.Player.Speed, Mathf.Abs(kinematicRigidbody2D.Velocity.x));
            player.CurrentController.animator.SetBool(Values.AnimationParameters.Player.Grounded, kinematicRigidbody2D.IsGrounded);
            //horizontalVelocity = Vector2.zero;
           // verticalVelocity = Vector2.zero;
           // ResetJumpCount();
        }

        private void FixedUpdate()
        {
            kinematicRigidbody2D.Velocity = horizontalVelocity * speed + verticalVelocity * jumpSpeed;
            horizontalVelocity = Vector2.zero;
            verticalVelocity = Vector2.zero;
            ResetJumpCount();
        }

        public void MoveRight()
        {
            horizontalVelocity = Vector2.right;
            player.playerHorizontalDirection = Vector2.right;
            player.DirectionFacingLeftRight = FacingSideLeftRight.Right;
            player.DirectionFacingUpDown = FacingSideUpDown.None;
            player.IsMoving = true;
        }

        public void MoveLeft()
        {
            horizontalVelocity = Vector2.left;
            player.playerHorizontalDirection = Vector2.left;
            player.DirectionFacingLeftRight = FacingSideLeftRight.Left;
            player.DirectionFacingUpDown = FacingSideUpDown.None;
            player.IsMoving = true;
        }

        public void Jump()
        {
            if (kinematicRigidbody2D.TimeSinceAirborne < playerNoLongerGroundedDelay && jumpCount == 0)
            {
                verticalVelocity = Vector2.up;
                player.CurrentController.animator.SetTrigger(Values.AnimationParameters.Player.Jump);
                player.IsMoving = true;
            }
            else if (jumpCount < amountOfAdditionalJumps)
            {
                verticalVelocity = Vector2.up* 0.76f;
                player.CurrentController.animator.SetTrigger(Values.AnimationParameters.Player.Jump);
                player.IsMoving = true;
                jumpCount++;
            }
        }

        public void AimUp()
        {
            player.DirectionFacingUpDown = FacingSideUpDown.Up;
            player.playerVerticalDirection = Vector2.up;
        }

        public void AimDown()
        {
            player.DirectionFacingUpDown = FacingSideUpDown.Down;
            player.playerVerticalDirection = Vector2.down;
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
