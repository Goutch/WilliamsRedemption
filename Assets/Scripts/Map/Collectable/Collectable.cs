using Harmony;
using UnityEngine;

namespace DefaultNamespace.Collectable
{
    public class Collectable:MonoBehaviour
    {
        [SerializeField] private int scoreValue;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == R.S.Tag.Player)
            {
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().AddScore(scoreValue);
                Destroy(this.gameObject);
            }
        }
    }
}