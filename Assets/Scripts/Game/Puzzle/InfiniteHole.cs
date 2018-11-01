using Game.Entity;
using Game.Entity.Player;
using Harmony;
using UnityEngine;

namespace Game.Puzzle
{
    public class InfiniteHole : MonoBehaviour
    {
        [SerializeField] private Transform respawnPoint;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.Root().CompareTag(Values.Tags.Player))
            {
                other.Root().transform.position = respawnPoint.position;
                other.transform.root.GetComponent<PlayerController>().DamagePlayer();
            }
            if (other.CompareTag(Values.Tags.Enemy))
            {
                other.GetComponent<Health>().Kill();
            }
        }
    }
}

