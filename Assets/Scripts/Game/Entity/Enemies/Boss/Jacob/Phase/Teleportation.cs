
using Game.Entity.Player;
using UnityEngine;

namespace Game.Entity.Enemies.Boss.Jacob
{
    class Teleportation : Capacity
    {
        private const float EQUALITY_POSITION_SENSIBILITY = 0.2f;

        [SerializeField] private float distanceFromPlayerTeleport;
        [SerializeField] private Transform[] teleportPoints;
        [SerializeField] private float cooldown;
        [SerializeField] private GameObject spawnParticulePrefab;
        
        [Header("Sound")] [SerializeField] private AudioClip teleportationSound;
        [SerializeField] private GameObject soundToPlayPrefab;
        private GameObject soundToPlay;
        
        private float lastUsed;

        private BossController bossController;

        protected override void Init()
        {
            bossController = GetComponent<BossController>();
        }

        public override void Act()
        {
            
        }

        public override bool CanEnter()
        {
            if ((Time.time - lastUsed > cooldown ||
                Vector2.Distance(player.transform.position, transform.position) <
                distanceFromPlayerTeleport) && !(bossController.GetCurrentState() is Vulnerable))
                return true;
            else
                return false;
        }

        public override void Enter()
        {
            base.Enter();

            Teleport();
            
            CallTeleportationSound();

            lastUsed = Time.time;

            Finish();
        }

        protected virtual void Teleport()
        {
            Vector2 oldPosition = transform.position;

            if(spawnParticulePrefab != null)
                Destroy(Instantiate(spawnParticulePrefab, transform.position, Quaternion.identity),3);

            do
            {
                transform.position = teleportPoints[Random.Range(0, teleportPoints.Length)].position;
            } while (Vector2.Distance(player.transform.position, transform.position) <
                     distanceFromPlayerTeleport
                     || Vector2.Distance(oldPosition, transform.position) < EQUALITY_POSITION_SENSIBILITY);
        }
        
        private void CallTeleportationSound()
        {
            soundToPlay=Instantiate(soundToPlayPrefab,transform.position,Quaternion.identity);
            soundToPlay.GetComponent<AudioManagerSpecificSounds>().Init(teleportationSound, false, gameObject);
            soundToPlay.GetComponent<AudioManagerSpecificSounds>().PlaySound();
        }
    }
}