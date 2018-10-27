using System.Collections;
using Game;
using Harmony;
using Playmode.EnnemyRework;
using UnityEngine;

namespace Enemies
{
    public class Sorcerer : WalkTowardPlayerEnnemy
    {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float sightRange;
        [SerializeField] float attackCooldown;
        private float timeSinceLastAttack = 0;
        
        

        protected override void Init()
        {
            base.Init();
            
        }

        protected override void FixedUpdate()
        {
            RaycastHit2D hit2D = new RaycastHit2D();
            Vector2 dir = PlayerController.instance.transform.position - transform.position;
            hit2D = Physics2D.Raycast(transform.position, dir, sightRange);
            if (hit2D.collider != null && hit2D.collider.tag == "Player")
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
            projectile.GetComponent<HitStimulus>().SetDamageSource(HitStimulus.DamageSourceType.Enemy);
            animator.SetTrigger("Attack");
        }

        private Quaternion PlayerDirection()
        {
            Vector2 dir = PlayerController.instance.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion direction = Quaternion.AngleAxis(angle, Vector3.forward);
            return direction;
        }
    }
}