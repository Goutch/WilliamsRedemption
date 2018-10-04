using System.Diagnostics.Eventing.Reader;
using UnityEngine;

namespace Playmode.EnnemyRework
{
    public class RootMover : MonoBehaviour
    {
        [SerializeField] float jumpForce;
        [SerializeField] float speed;
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

        public void WalkToward(int direction)
        {
            rootRigidBody.velocity = Vector2.up * rootRigidBody.velocity + (Vector2.right * direction * speed);
        }

        public void FlyToward(Vector2 targetPosition)
        {
            this.transform.root.position = Vector3.MoveTowards(this.transform.root.position, (Vector3) targetPosition,
                speed * Time.deltaTime);
        }

        public void Jump()
        {
            this.isJumping = true;
            this.rootRigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}