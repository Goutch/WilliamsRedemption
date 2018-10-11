using Boss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Jacob
{
    class Teleportation : Capacity
    {
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

        protected override void Initialise()
        {
            Teleport();

            lastUsed = Time.time;

            Finish();
        }

        private void Teleport()
        {
            do
            {
                transform.position = teleportPoints[UnityEngine.Random.Range(0, teleportPoints.Length)].position;
            } while (Vector2.Distance(PlayerController.instance.transform.position, transform.position) < distanceFromPlayerTeleport);
        }
    }
}
