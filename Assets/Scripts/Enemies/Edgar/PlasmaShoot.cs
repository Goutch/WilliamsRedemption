using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Edgar
{
    class PlasmaShoot : State
    {
        private EdgarController edgarController;
        private int numbOfPlasmaShoot;
        private int numbOfPlasmaShooted = 0;
        private float delayBetweenEachShoot;
        private float lastPlasmaShoot;

        public PlasmaShoot(int numbOfPlasmaShoot, float delayBetweenEachShoot)
        {
            this.delayBetweenEachShoot = delayBetweenEachShoot;
            this.numbOfPlasmaShoot = numbOfPlasmaShoot;
        }

        public void Act()
        {
            if(numbOfPlasmaShooted >= numbOfPlasmaShoot)
            {
                edgarController.OnPlasmaShootFinish();
            }
            else if (Time.time - lastPlasmaShoot > delayBetweenEachShoot)
            {
                lastPlasmaShoot = Time.time;
                numbOfPlasmaShooted++;

                Vector2 dir = PlayerController.instance.transform.position - edgarController.transform.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                Quaternion direction = Quaternion.AngleAxis(angle, Vector3.forward);

                edgarController.ShootPlasma(direction);
            }
        }

        public bool Init(EdgarController edgarController)
        {
            lastPlasmaShoot = Time.time - delayBetweenEachShoot;
            this.edgarController = edgarController;
            return true;
        }
    }
}
