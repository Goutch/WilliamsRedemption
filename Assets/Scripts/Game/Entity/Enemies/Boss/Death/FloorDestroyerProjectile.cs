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
    class FloorDestroyerProjectile : ProjectileController
    {
        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            base.OnCollisionEnter2D(collision);
            if (collision.gameObject.CompareTag(Values.Tags.Plateforme))
            {
                FloorTile floorTile;
                if (floorTile = collision.gameObject.GetComponent<FloorTile>())
                {
                    floorTile.MoveDown();
                }
            }
        }

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            base.OnTriggerEnter2D(other);
            if (other.CompareTag(Values.Tags.Plateforme))
            {
                FloorTile floorTile;
                if (floorTile = other.GetComponent<FloorTile>())
                {
                    floorTile.MoveDown();
                }

            }
        }
    }
}
