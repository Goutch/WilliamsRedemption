using Harmony;
using UnityEngine;

namespace DefaultNamespace
{
    public class Obstacle:MonoBehaviour
    {
        void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.collider.Root().GetComponent<PlayerController>().DamagePlayer();
            }
        }
    }
}