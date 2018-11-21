using Game.Controller;
using UnityEngine;

namespace Game.Puzzle
{
    public class Collectable : MonoBehaviour
    {
        [SerializeField] private int scoreValue;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.transform.root.CompareTag(Values.Tags.Player))
            {
                GameObject.FindGameObjectWithTag(Values.Tags.GameController).GetComponent<GameController>().AddCollectable(scoreValue);
                Destroy(this.gameObject);
            }
        }
    }
}