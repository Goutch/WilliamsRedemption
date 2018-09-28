using DefaultNamespace.Playmode;
using UnityEngine;

namespace Playmode.EnnemyRework
{
    public abstract class Enemy : EnemyData,IEntityData
    {
        private void Start()
        {
            GetComponent<HitSensor>().OnHit += ReceiveDamage;
        }

        public abstract void ReceiveDamage();
    }
}