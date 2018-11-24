using Game.Entity.Player;
using Harmony;
using UnityEngine;

namespace Game.Puzzle
{
    public class Obstacle : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.transform.root.gameObject.CompareTag(Values.Tags.Player))
            {
                other.collider.Root().GetComponent<PlayerController>().DamagePlayer(gameObject);
            }
        }
    }
}