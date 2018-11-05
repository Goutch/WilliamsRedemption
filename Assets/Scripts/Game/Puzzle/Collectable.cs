using Game.Controller;
using UnityEngine;

namespace Game.Puzzle
{
    public class Collectable : MonoBehaviour
    {
        [SerializeField] private int scoreValue;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(Values.Tags.Player))
            {
                //BEN_REVIEW : Le FindGameObjectWithTag et le GetComponent pourrait être fait au Awake à la place.
                //
                //             Juste par convention.
                GameObject.FindGameObjectWithTag(Values.Tags.GameController).GetComponent<GameController>().AddCollectable(scoreValue);
                Destroy(this.gameObject);
            }
        }
    }
}