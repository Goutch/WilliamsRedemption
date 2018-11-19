using Game.Controller.Events;
using UnityEngine;
using XInputDotNetPure;


namespace Game.Entity.Player
{
    public class Mover : MonoBehaviour
    {
        [Tooltip("Player mouvement speed")] [SerializeField]
        private float speed;

        [Tooltip("Player Jump Speed")] [SerializeField]
        private float jumpSpeed;

        [Tooltip("Short period of time where the player can jump while being airborne.")] [SerializeField]
        private float playerNoLongerGroundedDelay;

        [Tooltip("Amount of jumps allowed after leaving ground.")] [SerializeField]
        private float amountOfAdditionalJumps;

        [Tooltip("Jump velocity muliplier. Only effective after the first jump.")] [SerializeField]
        private float additionalJumpVelocity;
        
        [SerializeField] private AudioClip jumpSound;
        [SerializeField] private GameObject soundToPlayPrefab;
        private GameObject soundToPlay;

        
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

        private PlayerJumpEventChannel jumpEventChannel;

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
            jumpEventChannel = GameObject.FindGameObjectWithTag(Values.GameObject.GameController)
                .GetComponent<PlayerJumpEventChannel>();
        }

        private void Update()
        {
            velocityY = this.transform.position.y - lastPositionY;
            velocityY = velocityY / Time.fixedDeltaTime;
            lastPositionY = this.transform.position.y;
            player.CurrentController.animator.SetFloat(Values.AnimationParameters.Player.VelocityY, velocityY);
            player.CurrentController.animator.SetFloat(Values.AnimationParameters.Player.Speed,
                Mathf.Abs(kinematicRigidbody2D.Velocity.x));
            player.CurrentController.animator.SetBool(Values.AnimationParameters.Player.Grounded,
                kinematicRigidbody2D.IsGrounded);
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
            //player.DirectionFacingLeftRight = FacingSideLeftRight.Right;
            //player.DirectionFacingUpDown = FacingSideUpDown.None;
        }

        public void MoveLeft()
        {
            horizontalVelocity = Vector2.left;
            player.playerHorizontalDirection = Vector2.left;
           // player.DirectionFacingLeftRight = FacingSideLeftRight.Left;
           // player.DirectionFacingUpDown = FacingSideUpDown.None;
        }

        public void Jump()
        {
            if (kinematicRigidbody2D.TimeSinceAirborne < playerNoLongerGroundedDelay && jumpCount == 0)
            {
                UseSound();
                verticalVelocity = Vector2.up;
                player.CurrentController.animator.SetTrigger(Values.AnimationParameters.Player.Jump);
            }
            else if (jumpCount < amountOfAdditionalJumps)
            {
                UseSound();
                verticalVelocity = Vector2.up * additionalJumpVelocity;
                player.CurrentController.animator.SetTrigger(Values.AnimationParameters.Player.Jump);
                jumpCount++;
            }
            jumpEventChannel.Publish(new OnPlayerJump());
            
        }

        

        private void ResetJumpCount()
        {
            if (kinematicRigidbody2D.IsGrounded)
            {
                jumpCount = 0;
            }
        }
        
        private void UseSound()
        {
            soundToPlay=Instantiate(soundToPlayPrefab,this.transform.position,Quaternion.identity);
            soundToPlay.GetComponent<AudioManagerSpecificSounds>().Init(jumpSound, true, this.gameObject);
            soundToPlay.GetComponent<AudioManagerSpecificSounds>().PlaySound();
        }
    }
}