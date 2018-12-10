using Game.Entity.Enemies.Attack;
using Game.Entity.Player;
using Harmony;
using UnityEngine;

namespace Game.Puzzle
{
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] private GameObject DmgEffect;
        
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.transform.root.gameObject.CompareTag(Values.Tags.Player))
            {
                PlayerController player = other.collider.Root().GetComponent<PlayerController>();
                player.DamagePlayer(gameObject);
                if (player.CurrentController is WilliamController)
                {
                    Bleed(other);
                }
            }      
        }

        private void Bleed(Collision2D other)
        {
            if (DmgEffect != null)
            {
                Destroy(Instantiate(DmgEffect, other.transform.position, other.transform.rotation), 3);
            }                
        }
    }
}