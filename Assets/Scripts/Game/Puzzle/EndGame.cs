using Game.Controller;
using Harmony;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Puzzle
{
    public class EndGame : MonoBehaviour
    {
        private GameController gameController;
        private void Start()
        {
            gameController = GameObject.FindGameObjectWithTag(Values.Tags.GameController).GetComponent<GameController>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(Values.Tags.Player))
            {
                gameController.NextLevel();
            }
        }
    }
}