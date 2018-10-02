using DefaultNamespace.Playmode;
using UnityEngine;

namespace Playmode.EnnemyRework
{
    public abstract class Enemy : EnemyData
    {
        private void Awake()
        {
            GetComponent<HitSensor>().OnHit += ReceiveDamage;
            Init();
        }

        protected abstract void Init();
        public abstract void ReceiveDamage();
    }
}