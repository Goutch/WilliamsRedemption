using Game.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Puzzle
{
    //BEN_REVIEW : On a un léger problème de conception ici. Voir mes commentaires dans "EnemySpawner".
    //
    //             À mon avis, c'est pas uniquement relié au "Ghosts". Cela pourait être reliés à n'importe
    //             quel autre type d'ennemi.
    class GhostSpawner : EnemySpawner
    {
        protected override void SpawnEnemy()
        {
            base.SpawnEnemy();

            spawnedEnemy.GetComponent<Health>().OnDeath += GhostSpawner_OnDeath;
        }

        private void GhostSpawner_OnDeath(GameObject receiver, GameObject attacker)
        {
            spawnedEnemy.GetComponent<Health>().OnDeath -= GhostSpawner_OnDeath;
            if (!attacker.CompareTag(Values.Tags.Player))
            {
                spawned = false;
            }
        }
    }
}