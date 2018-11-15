
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Entity.Enemies.Boss
{
    class ProjectileManager : MonoBehaviour
    {
        private List<GameObject> projectiles;

        private void Awake()
        {
            projectiles = new List<GameObject>();
        }

        public GameObject SpawnProjectile(GameObject projectile, Vector2 position, Quaternion rotation)
        {
            GameObject projectileObject = Instantiate(projectile, position, rotation);
            projectiles.Add(projectileObject);
            return projectileObject;
        }

        public void Clear()
        {
            foreach (GameObject projectile in projectiles)
                Destroy(projectile);

            projectiles.Clear();
        }

    }
}
