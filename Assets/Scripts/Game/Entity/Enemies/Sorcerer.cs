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
        [SerializeField] private float delayBeforeShootingAfterSeeingPlayer;
        [SerializeField] private float attackCooldown;


        private new Rigidbody2D rigidbody;
        private float timeSinceLastAttack;
        private bool knocked;

        private float? lastTimePlayerSeeing = null;

        protected override void Init()
        {
            base.Init();
            timeSinceLastAttack = Time.time;
            rigidbody = GetComponent<Rigidbody2D>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            OnPlayerSightEnter += Sorcerer_OnPlayerSightEnter;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            OnPlayerSightEnter -= Sorcerer_OnPlayerSightEnter;
        }

        private void Sorcerer_OnPlayerSightEnter()
        {
            lastTimePlayerSeeing = Time.time;
        }

        protected override void FixedUpdate()
        {
            if (!knocked)
            {
                base.FixedUpdate();

                if (IsPlayerInSight())
                {
                    if ((Time.time - timeSinceLastAttack > attackCooldown ) && lastTimePlayerSeeing.HasValue && (Time.time - delayBeforeShootingAfterSeeingPlayer > lastTimePlayerSeeing.Value))
                    {
                        ShootProjectile(PlayerDirection());
                        timeSinceLastAttack = Time.time;
                    }
                }
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
            Vector2 dir = player.transform.position - (transform.position - new Vector3(0,0.1f,0));
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion direction = Quaternion.AngleAxis(angle, Vector3.forward);
            return direction;
        }

        protected override bool OnHit(HitStimulus hitStimulus)
        {
            if (hitStimulus.Type == HitStimulus.DamageType.Darkness)
            {
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