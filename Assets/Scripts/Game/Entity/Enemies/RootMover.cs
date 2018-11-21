using AnimatorExtension;
using Game.Entity.Player;
using UnityEngine;

namespace Game.Entity.Enemies
{
    public class RootMover : MonoBehaviour
    {
        [SerializeField] private float jumpForce;
        [SerializeField] private float speed;
        [SerializeField] private float jumpBoost;
        private int currentDir;
        private bool isJumping = false;

        public bool IsJumping => isJumping;
        private Animator animator;

        public float Speed
        {
            get { return speed; }

            set { speed = value; }
        }

        private Rigidbody2D rootRigidBody;

        private void Awake()
        {
            this.rootRigidBody = this.transform.root.GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }

        private void FixedUpdate()
        {
            if (this.isJumping && this.rootRigidBody.velocity.y == 0.0)
            {
                this.isJumping = false;
                if (animator.ContainsParam(Values.AnimationParameters.Enemy.JumpEnd))
                    animator.SetTrigger(Values.AnimationParameters.Enemy.JumpEnd);
            }
        }

        public void WalkToward(int direction)
        {
            if (direction != 0)
            {
                rootRigidBody.velocity = Vector2.up * rootRigidBody.velocity + (Vector2.right * direction * speed);
                animator.SetBool(Values.AnimationParameters.Enemy.Walking, true);
            }
            else
            {
                animator.SetBool(Values.AnimationParameters.Enemy.Walking, false);
                rootRigidBody.velocity *= Vector2.up;
            }

            currentDir = direction;
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
            this.rootRigidBody.AddForce(new Vector2(currentDir * jumpBoost, jumpForce), ForceMode2D.Impulse);
            
            animator.SetTrigger(Values.AnimationParameters.Enemy.Jump);
        }

        public void LookAtPlayer()
        {
            LookAtPlayer(transform.position);
        }

        public void LookAtPlayer(Vector2 fromPosition)
        {
            float directionX = Mathf.Sign(PlayerController.instance.transform.position.x - fromPosition.x);
            if (directionX < 0)
                transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
            else
                transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
        }
    }
}