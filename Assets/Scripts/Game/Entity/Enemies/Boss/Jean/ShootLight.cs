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
        
        [Header("Sound")] [SerializeField] private AudioClip shootLaserSound;
        [SerializeField] private GameObject soundToPlayPrefab;

        private ShieldManager shieldManager;
        private Animator animator;

        private float lastTimeUsed;
        private bool isLeftHandShootTurn = true;

        protected override void Init()
        {
            animator = GetComponent<Animator>();
        }

        public override void Act()
        {
        }

        public override void Enter()
        {
            base.Enter();

            shieldManager = GetComponent<ShieldManager>();

            if(isLeftHandShootTurn)
                animator.SetTrigger(Values.AnimationParameters.Jean.LeftShoot);
            else
                animator.SetTrigger(Values.AnimationParameters.Jean.RightShoot);

            isLeftHandShootTurn = !isLeftHandShootTurn;

            int random = Random.Range(0, 100);
            Vector2 spawnPosition = random < probabilitySpawn1
                ? projectileSpawnPoint1.transform.position
                : projectileSpawnPoint2.transform.position;
            Instantiate(projectile, spawnPosition, transform.rotation);
            
            Audio.SoundCaller.CallSound(shootLaserSound, soundToPlayPrefab, gameObject, false);

            shieldManager.UseShield(shieldCost);

            Finish();
        }

        public override bool CanEnter()
        {
            return true;
        }
    }
}