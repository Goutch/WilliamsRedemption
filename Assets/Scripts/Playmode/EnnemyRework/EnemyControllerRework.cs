using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.Experimental.Rendering;

namespace Playmode.EnnemyRework
{
    public class EnemyControllerRework : MonoBehaviour
    {
        [SerializeField] private Enemy enemy;
        private Health health;
        private void Awake()
        {
            enemy.Init(gameObject);
            GetComponent<SpriteRenderer>().sprite = enemy.Sprite;
            gameObject.AddComponent<BoxCollider2D>();
            health = this.GetComponent<Health>();
           
        }

        private void FixedUpdate()
        {
            enemy.Act();
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
    }
}