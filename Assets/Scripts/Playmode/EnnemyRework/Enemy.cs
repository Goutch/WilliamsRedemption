using DefaultNamespace.Playmode;
using UnityEngine;

namespace Playmode.EnnemyRework
{
    public abstract class Enemy : EnemyData, IEnemyStrategy
    {
        public abstract void Init(GameObject enemyControllerObject);
        public abstract void Act();

        public virtual void ReactTriggerEnter(Collider2D other)
        {
        }

        public virtual void ReactCollisionEnter(Collision2D other)
        {
        }

        public virtual void ReactToColision(Collision2D collision2D)
        {
        }
    }
}