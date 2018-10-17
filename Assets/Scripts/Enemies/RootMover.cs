using System.Diagnostics.Eventing.Reader;
using UnityEngine;

namespace Playmode.EnnemyRework
{
    public class RootMover : MonoBehaviour
    {
        [SerializeField] private float jumpForce;
        [SerializeField] private float speed;
        private bool isJumping = false;

        public bool IsJumping => isJumping;
        private Animator animator;
        public float Speed
        {
            get
            {
                return speed;
            }

            set
            {
                speed = value;
            }
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
                animator.SetTrigger("JumpEnd");
            }
                
        }

        public void WalkToward(int direction)
        {
            if (direction != 0)
            {
                rootRigidBody.velocity = Vector2.up * rootRigidBody.velocity + (Vector2.right * direction * speed);
            }
            else
            {
                rootRigidBody.velocity = Vector2.zero;
            }
        }

        public void FlyToward(Vector2 targetPosition)
        {
            this.transform.root.position = Vector3.MoveTowards(this.transform.root.position, (Vector3) targetPosition,
                Speed * Time.deltaTime);
        }

        public void MoveOnXAxis(int direction)
        {
            rootRigidBody.velocity = new Vector2(Vector2.up.x * rootRigidBody.velocity.x + (Vector2.right.x * direction * Speed), rootRigidBody.velocity.y);
        }

        public void MoveOnXAxis()
        {
            MoveOnXAxis(1);
        }

        public void Jump()
        {
            this.isJumping = true;
            this.rootRigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator.SetTrigger("Jump");
        }

        public void LookAtPlayer()
        {
            float directionX = Mathf.Sign(PlayerController.instance.transform.position.x - transform.position.x);
            if (directionX > 0)
                transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
            else
                transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
        }
    }
}