using Harmony;
using UnityEngine;

namespace DefaultNamespace
{
    public class Obstacle:MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.tag == R.E.Tag.Player.ToString())
            {
                other.collider.Root().GetComponent<PlayerController>().DamagePlayer();
            }
        }
    }
}