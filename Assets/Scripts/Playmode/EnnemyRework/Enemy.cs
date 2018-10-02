using System.Security.AccessControl;
using DefaultNamespace.Playmode;
using Harmony;
using UnityEngine;

namespace Playmode.EnnemyRework
{

    public abstract class Enemy : EnemyData,IEntityData
    {
        
        protected Health health;

        private void Awake()
        {
            Init();
            health=GetComponent<Health>();
        }

        protected abstract void Init();

        public virtual void ReceiveDamage(Collider2D other)
        {
            health.Hit();
        }

        protected virtual void  OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "ProjectilePlayer")
            {
                ReceiveDamage(other);
            } 
        }

        protected virtual void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.tag == "Player")
            {
                other.collider.Root().GetComponent<PlayerController>().DamagePlayer();
            }
        }
    }
}