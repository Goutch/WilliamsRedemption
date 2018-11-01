using Game.Entity.Player;
using Harmony;
using UnityEngine;

namespace Game.Puzzle
{
    public class Obstacle : MonoBehaviour
    {
        void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag(Values.Tags.Player))
            {
                other.collider.Root().GetComponent<PlayerController>().DamagePlayer();
            }
        }
    }
}