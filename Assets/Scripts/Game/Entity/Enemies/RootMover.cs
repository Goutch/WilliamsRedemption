using Game.Entity.Player;
using UnityEngine;

namespace Game.Entity.Enemies
{
    public class RootMover : MonoBehaviour
    {
        [SerializeField] private float jumpForce;
        [SerializeField] private float speed;
        [SerializeField] private float jumpBoost;

        public float Speed
        {
            get { return speed; }

            set { speed = value; }
        }

        private int currentDir;
        private bool isJumping = false;
        public bool IsJumping => isJumping;

        private Animator animator;
        private Rigidbody2D rootRigidBody;
        private PlayerController player;

        private void Awake()
        {
            this.rootRigidBody = this.transform.root.GetComponent<Rigidbody2D>();
            player = GameObject.FindGameObjectWithTag(Values.Tags.Player).GetComponent<PlayerController>();
            animator = GetComponent<Animator>();
        }

        private void FixedUpdate()
        {
            Debug.Log(this.rootRigidBody.velocity.y);
            if (this.isJumping && this.rootRigidBody.velocity.y == 0.0)
            {
                
                this.isJumping = false;
                animator.SetTrigger(Values.AnimationParameters.Enemy.JumpEnd);
            }
        }

        public void Walk()
        {
            int direction = (transform.rotation.y == 1 || transform.rotation.y == -1 ? -1 : 1);

            rootRigidBody.velocity = new Vector2(speed * direction, rootRigidBody.velocity.y);
            animator.SetBool(Values.AnimationParameters.Enemy.Walking, true);
        }

        public void StopWalking()
        {
            rootRigidBody.velocity = new Vector2(0, rootRigidBody.velocity.y);
        }

        public void FlyToward(Vector2 targetPosition)
        {
            this.transform.root.position = Vector3.MoveTowards(this.transform.root.position, (Vector3) targetPosition,
                Speed * Time.deltaTime);
        }

        public void MoveOnXAxis(int direction)
        {
            rootRigidBody.velocity =
                new Vector2(Vector2.up.x * rootRigidBody.velocity.x + (Vector2.right.x * direction * Speed),
                    rootRigidBody.velocity.y);
        }

        public void MoveForward()
        {
            rootRigidBody.MovePosition(new Vector2(
                transform.position.x + Speed * Time.deltaTime * (transform.rotation.y == 1 ? -1 : 1),
                transform.position.y));
        }

        public void MoveOnXAxis()
        {
            MoveOnXAxis(1);
        }

        public void Jump()
        {
            this.isJumping = true;
            int direction = (transform.rotation.y == 1 || transform.rotation.y == -1 ? -1 : 1);
            this.rootRigidBody.AddForce(new Vector2(direction * jumpBoost, jumpForce), ForceMode2D.Impulse);
            animator.SetTrigger(Values.AnimationParameters.Enemy.Jump);
        }

        public void LookAtPlayer()
        {
            LookAtPlayer(transform.position);
        }

        public void LookAtPlayer(Vector2 fromPosition)
        {
            float directionX = Mathf.Sign(player.transform.position.x - fromPosition.x);
            if (directionX < 0)
                transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
            else
                transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
        }
    }
}