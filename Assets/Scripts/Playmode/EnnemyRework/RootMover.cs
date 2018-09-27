using UnityEngine;

namespace Playmode.EnnemyRework
{
    public class RootMover : MonoBehaviour
    {
        private bool isJumping = false;
        private Rigidbody2D rootRigidBody;

        private void Awake()
        {
            this.rootRigidBody = this.transform.root.GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (!this.isJumping || (double) this.rootRigidBody.velocity.y != 0.0)
                return;
            this.isJumping = false;
        }

        public void WalkToward(int direction ,float speed)
        {
            this.transform.root.Translate(Vector3.right * (float) direction * speed * Time.deltaTime);
        }

        public void FlyToward(Vector2 targetPosition,float speed)
        {
            this.transform.root.position = Vector3.MoveTowards(this.transform.root.position, (Vector3) targetPosition, speed * Time.deltaTime);
        }

        public void Jump(Vector2 jumpForce)
        {
            this.isJumping = true;
            this.rootRigidBody.AddForce(jumpForce, ForceMode2D.Impulse);
        }
    }
}
