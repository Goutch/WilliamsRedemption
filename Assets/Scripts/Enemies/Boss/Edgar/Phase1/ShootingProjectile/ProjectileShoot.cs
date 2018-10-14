using UnityEngine;

namespace Playmode.EnnemyRework.Boss.Edgar
{
    class ProjectileShoot : Capacity
    {
        [SerializeField] private float cooldown;
        [SerializeField] private bool capacityUsableAtStart;
        [SerializeField] private GameObject bullet;

        private float lastTimeCapacityUsed;

        private void Awake()
        {
            if (capacityUsableAtStart)
                lastTimeCapacityUsed = -cooldown;
        }

        public override void Act()
        {

        }

        public override bool CanEnter()
        {
            if (Time.time - lastTimeCapacityUsed > cooldown)
                return true;
            else
                return false;
        }
        public override void Enter()
        {
            base.Enter();

            lastTimeCapacityUsed = Time.time;

            ShootProjectile(PlayerDirection());

            Finish();
        }

        public void ShootProjectile(Quaternion direction)
        {
            GameObject projectile = Instantiate(bullet, transform.position, direction);
            projectile.GetComponent<HitStimulus>().SetDamageSource(HitStimulus.DamageSourceType.Enemy);
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
