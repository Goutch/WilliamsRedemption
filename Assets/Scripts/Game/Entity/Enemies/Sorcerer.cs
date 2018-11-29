using Game.Entity.Enemies.Attack;
using Game.Entity.Player;
using Harmony;
using UnityEngine;

namespace Game.Entity.Enemies
{
    public class Sorcerer : WalkTowardPlayerEnnemy
    {
        [SerializeField] private Vector2 bulletKnockBackForce;
        [SerializeField] private Vector2 DarknessKnockBackForce;
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private GameObject spawnProjectilePoint;
        [SerializeField] private float sightRange;
        [SerializeField] private float attackCooldown;

        [SerializeField] private LayerMask layerVisibleByTheUnit;
        private Rigidbody2D rigidbody;
        private float timeSinceLastAttack;
        private bool knocked;

        protected override void Init()
        {
            base.Init();
            timeSinceLastAttack = Time.time;
            rigidbody = GetComponent<Rigidbody2D>();
        }

        protected override void FixedUpdate()
        {
            if (!knocked)
            {
                base.FixedUpdate();

                RaycastHit2D hit2D = new RaycastHit2D();

                Vector2 dir = player.transform.position - transform.position;
                hit2D = Physics2D.Raycast(transform.position, dir, sightRange, layerVisibleByTheUnit);

                //if (hit2D.collider != null && hit2D.collider.transform.root.CompareTag(Values.Tags.Player))
                //{
                    if (Time.time - timeSinceLastAttack > attackCooldown)
                    {
                        ShootProjectile(PlayerDirection());
                        timeSinceLastAttack = Time.time;
                    }
                //}
            }

            else if (knocked && rigidbody.velocity.y == 0)
                knocked = false;
        }

        public void ShootProjectile(Quaternion direction)
        {
            GameObject projectile = Instantiate(projectilePrefab, spawnProjectilePoint.transform.position, direction);
            animator.SetTrigger(Values.AnimationParameters.Enemy.Attack);
        }

        private Quaternion PlayerDirection()
        {
            Vector2 dir = player.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion direction = Quaternion.AngleAxis(angle, Vector3.forward);
            return direction;
        }

        protected override bool OnHit(HitStimulus hitStimulus)
        {
            if (hitStimulus.Type == HitStimulus.DamageType.Darkness)
            {
                base.OnHit(hitStimulus);
                int direction = (transform.position.x-hitStimulus.transform.position.x > 0 ? -1 : 1);

                rigidbody.AddForce(new Vector2(DarknessKnockBackForce.x * -direction, DarknessKnockBackForce.y),
                    ForceMode2D.Impulse);
                knocked = true;
            }
            else if (hitStimulus.Type == HitStimulus.DamageType.Physical)
            {
                int direction = (transform.position.x-hitStimulus.Root().transform.position.x > 0 ? -1 : 1);

                rigidbody.AddForce(new Vector2(bulletKnockBackForce.x * -direction, bulletKnockBackForce.y),
                    ForceMode2D.Impulse);
                knocked = true;
            }

            return base.OnHit(hitStimulus);
        }
    }
}