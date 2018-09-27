using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.Experimental.Rendering;

namespace Playmode.EnnemyRework
{
    public class EnemyControllerRework : MonoBehaviour
    {
        [SerializeField] private Enemy enemyStrategy;

        public Enemy EnemyStrategy
        {
            get { return enemyStrategy; }
            set { enemyStrategy = Instantiate(value); }
        }

        private Health health;

        public void Init(Enemy enemyStrategy)
        {
            EnemyStrategy = enemyStrategy;
            GetComponent<SpriteRenderer>().sprite = EnemyStrategy.Sprite;
            gameObject.AddComponent<BoxCollider2D>();
            EnemyStrategy.Init(gameObject);
            health = this.GetComponent<Health>();
        }

        private void FixedUpdate()
        {
            enemyStrategy.Act();
        }

        private void OnHit(int hitPoints)
        {
            health.Hit(hitPoints);
            Debug.Log(health.HealthPoints);
            OnHealthChange(health.HealthPoints);
        }

        private void OnHealthChange(int newHealth)
        {
            if (newHealth <= 0)
            {
                Destroy(this.gameObject);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            enemyStrategy.ReactCollisionEnter(other);
           
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            enemyStrategy.ReactTriggerEnter(other);
        }
    }
}