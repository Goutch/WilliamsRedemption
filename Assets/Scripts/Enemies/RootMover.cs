using System.Diagnostics.Eventing.Reader;
using UnityEngine;

namespace Playmode.EnnemyRework
{
    public class RootMover : MonoBehaviour
    {
        private bool isJumping = false;

        public bool IsJumping => isJumping;

        private Rigidbody2D rootRigidBody;

        private void Awake()
        {
            this.rootRigidBody = this.transform.root.GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (this.isJumping &&this.rootRigidBody.velocity.y == 0.0)
                this.isJumping = false;
        }

        public void WalkToward(int direction, float speed)
        {
            rootRigidBody.velocity = Vector2.up * rootRigidBody.velocity + (Vector2.right * direction * speed);
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
        }
    }
}