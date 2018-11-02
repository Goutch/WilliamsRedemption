
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
        private float lastUsed;

        private BossController bossController;

        private void Awake()
        {
            bossController = GetComponent<BossController>();
        }

        public override void Act()
        {
            
        }

        public override bool CanEnter()
        {
            if ((Time.time - lastUsed > cooldown ||
                Vector2.Distance(PlayerController.instance.transform.position, transform.position) <
                distanceFromPlayerTeleport) && !(bossController.GetCurrentState() is Vulnerable))
                return true;
            else
                return false;
        }

        public override void Enter()
        {
            base.Enter();

            Teleport();

            lastUsed = Time.time;

            Finish();
        }

        private void Teleport()
        {
            Vector2 oldPosition = transform.position;
            Destroy(Instantiate(spawnParticulePrefab, transform.position, Quaternion.identity),3);
            do
            {
                transform.position = teleportPoints[Random.Range(0, teleportPoints.Length)].position;
            } while (Vector2.Distance(PlayerController.instance.transform.position, transform.position) <
                     distanceFromPlayerTeleport
                     || Vector2.Distance(oldPosition, transform.position) < EQUALITY_POSITION_SENSIBILITY);
        }
        
    }
}