using UnityEngine;

namespace Playmode.EnnemyRework.Boss.Jacob
{
    class Teleportation : Capacity
    {
        private const float EQUALITY_POSITION_SENSIBILITY = 0.2f;

        [SerializeField] private float distanceFromPlayerTeleport;
        [SerializeField] private Transform[] teleportPoints;
        [SerializeField] private float cooldown;

        private float lastUsed;

        public override void Act()
        {
            
        }

        public override bool CanEnter()
        {
            if (Time.time - lastUsed > cooldown || Vector2.Distance(PlayerController.instance.transform.position, transform.position) < distanceFromPlayerTeleport)
                return true;
            else
                return false;
        }
        public override void Enter()
        {
            Teleport();

            lastUsed = Time.time;

            Finish();
        }

        private void Teleport()
        {
            Vector2 oldPosition = transform.position;

            do
            {

                transform.position = teleportPoints[Random.Range(0, teleportPoints.Length)].position;

            } while (Vector2.Distance(PlayerController.instance.transform.position, transform.position) < distanceFromPlayerTeleport 
            || Vector2.Distance(oldPosition, transform.position) < EQUALITY_POSITION_SENSIBILITY);
        }
    }
}
