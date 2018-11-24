using Game.Entity.Enemies.Attack;
using Game.Puzzle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Entity.Enemies.Boss.Death
{
    class FloorDestroyerProjectile : PlasmaController
    {
        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(Values.Tags.MovingPlatform))
            {
                FloorTile floorTile;
                if (floorTile = collision.GetComponent<FloorTile>())
                    floorTile.MoveDown();
            }

            base.OnTriggerEnter2D(collision);
        }
    }
}