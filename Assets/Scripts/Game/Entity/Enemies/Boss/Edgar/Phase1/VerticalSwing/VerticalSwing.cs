using UnityEngine;

namespace Game.Entity.Enemies.Boss.Edgar
{
    [RequireComponent(typeof(SpawnedTilesManager))]
    class VerticalSwing : Capacity
    {
        [Header("Config")]
        [Tooltip("Use Trigger '" + Values.AnimationParameters.Edgar.VerticalSwing + "' ")]
        [SerializeField] private Animator animator;
        [SerializeField] private float cooldown;
        [SerializeField] private bool capacityUsableAtStart;

        [Header("Projectile")]
        [SerializeField] private GameObject projectile;
        [SerializeField] private GameObject projectileSpawnPoint;

        private float lastTimeUsed;

        private SpawnedTilesManager spawnedTilesManager;

        private void Awake()
        {
            if (capacityUsableAtStart)
                lastTimeUsed = -cooldown;

            spawnedTilesManager = GetComponent<SpawnedTilesManager>();
        }

        public override void Act() { }

        public override bool CanEnter()
        {
            if (Time.time - lastTimeUsed > cooldown)
                return true;
            else
                return false;
        }
        public override void Enter()
        {
            base.Enter();

            animator.SetTrigger(Values.AnimationParameters.Edgar.VerticalSwing);
            lastTimeUsed = Time.time;
        }

        public void OnVerticalSwingFinish()
        {
            ShootProjectile();
            base.Finish();
        }

        public void ShootProjectile()
        {
            GameObject projectileObject = Instantiate(projectile, projectileSpawnPoint.transform.position, transform.rotation);

            projectileObject.GetComponent<PlasmaGroundController>().Init(spawnedTilesManager);
        }
    }
}
