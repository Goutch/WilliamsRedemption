using Game.Controller;
using UnityEngine;

namespace Game.Puzzle
{
    //BEN_REVIEW : J'aurais appellé ça "EndGameTrigger".
    public class EndGame : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(Values.Tags.Player))
            {
                GameObject.FindWithTag(Values.Tags.GameController).GetComponent<GameController>().OnGameEnd();
            }
        }
    }
}


