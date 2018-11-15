
using Game.Entity.Player;
using UnityEngine;

namespace Game.Entity.Enemies
{
    public class Sorcerer : WalkTowardPlayerEnnemy
    {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float sightRange;
        [SerializeField] private float attackCooldown;

        [SerializeField] private LayerMask layerVisibleByTheUnit;

        private float timeSinceLastAttack;

        protected override void Init()
        {
            base.Init();
            timeSinceLastAttack = Time.time;
        }

        protected override void FixedUpdate()
        {
            RaycastHit2D hit2D = new RaycastHit2D();

            Vector2 dir = player.transform.position - transform.position;
            hit2D = Physics2D.Raycast(transform.position, dir, sightRange, layerVisibleByTheUnit);

            if (hit2D.collider != null && hit2D.collider.CompareTag(Values.Tags.Player))
            {
                UpdateDirection();
                if (Time.time - timeSinceLastAttack > attackCooldown)
                {
                    ShootProjectile(PlayerDirection());
                    timeSinceLastAttack = Time.time;
                }

                if (rootMover.IsJumping == false)
                    rootMover.WalkToward(0);
            }
            else
            {
                base.FixedUpdate();
            }
        }

        public void ShootProjectile(Quaternion direction)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, direction);
            animator.SetTrigger(Values.AnimationParameters.Enemy.Attack);
        }

        private Quaternion PlayerDirection()
        {
            Vector2 dir =player.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion direction = Quaternion.AngleAxis(angle, Vector3.forward);
            return direction;
        }
    }
}