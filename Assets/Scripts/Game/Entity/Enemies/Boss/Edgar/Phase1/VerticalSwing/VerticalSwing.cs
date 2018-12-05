using Game.Audio;
using UnityEngine;

namespace Game.Entity.Enemies.Boss.Edgar
{
    [RequireComponent(typeof(SpawnedTilesManager))]
    class VerticalSwing : Capacity
    {
        [Header("Config")]
        [Tooltip("Use Trigger '" + Values.AnimationParameters.Edgar.VerticalSwing + "' ")]
        [SerializeField]
        private Animator animator;

        [SerializeField] private float cooldown;
        [SerializeField] private bool capacityUsableAtStart;

        [Header("Projectile")] [SerializeField]
        private GameObject projectile;

        [SerializeField] private GameObject projectileSpawnPoint;

        [Header("Sound")] [SerializeField] private AudioClip verticalSwingSound;
        [SerializeField] private GameObject soundToPlayPrefab;

        private float lastTimeUsed;

        private SpawnedTilesManager spawnedTilesManager;
        private RootMover mover;

        protected override void Init()
        {
            if (capacityUsableAtStart)
                lastTimeUsed = -cooldown;

            spawnedTilesManager = GetComponent<SpawnedTilesManager>();
            mover = GetComponent<RootMover>();
        }

        public override void Act()
        {
        }

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

            mover.LookAtPlayer();

            animator.SetTrigger(Values.AnimationParameters.Edgar.VerticalSwing);
            
            lastTimeUsed = Time.time;
        }

        public void OnVerticalSwingFinish()
        {
            SoundCaller.CallSound(verticalSwingSound, soundToPlayPrefab, gameObject, false);
            ShootProjectile();
            base.Finish();
        }

        private void ShootProjectile()
        {
            GameObject projectileObject =
                Instantiate(projectile, projectileSpawnPoint.transform.position, transform.rotation);

            projectileObject.GetComponent<PlasmaGroundController>().Init(spawnedTilesManager);
        }
    }
}