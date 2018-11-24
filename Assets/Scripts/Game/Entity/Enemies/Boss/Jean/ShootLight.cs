using UnityEngine;

namespace Game.Entity.Enemies.Boss.Jean
{
    class ShootLight : Capacity
    {
        [SerializeField] private bool capacityUsableAtStart;
        [SerializeField] private float shieldCost;
        [SerializeField] private GameObject projectile;
        [SerializeField] private GameObject projectileSpawnPoint1;
        [SerializeField] private GameObject projectileSpawnPoint2;
        [SerializeField] private float probabilitySpawn1;

        private ShieldManager shieldManager;

        private float lastTimeUsed;

        protected override void Init()
        {
        }

        public override void Act()
        {
        }

        public override void Enter()
        {
            base.Enter();

            shieldManager = GetComponent<ShieldManager>();

            int random = Random.Range(0, 100);
            Vector2 spawnPosition = random < probabilitySpawn1
                ? projectileSpawnPoint1.transform.position
                : projectileSpawnPoint2.transform.position;
            Instantiate(projectile, spawnPosition, transform.rotation);

            shieldManager.UseShield(shieldCost);

            Finish();
        }

        public override bool CanEnter()
        {
            return true;
        }
    }
}