using System.Diagnostics.Eventing.Reader;
using UnityEngine;

namespace Playmode.EnnemyRework
{
    public class RootMover : MonoBehaviour
    {
        private bool isJumping = false;

        public bool IsJumping => isJumping;
        private Animator animator;
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

        public void WalkToward(int direction, float speed)
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

        public void FlyToward(Vector2 targetPosition, float speed)
        {
            this.transform.root.position = Vector3.MoveTowards(this.transform.root.position, (Vector3) targetPosition,
                speed * Time.deltaTime);
        }

        public void Jump(Vector2 jumpForce)
        {
            this.isJumping = true;
            this.rootRigidBody.AddForce(jumpForce, ForceMode2D.Impulse);
            animator.SetTrigger("Jump");
        }
    }
}