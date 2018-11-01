using Game.Controller;
using Harmony;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Puzzle
{
    public class EndGame : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(Values.Tags.Player))
            {
                ChooseActionToDo();
            }
        }

        private void ChooseActionToDo()
        {
            if (SceneManager.GetActiveScene().name == R.S.Scene.Level1)
            {
                SceneManager.LoadScene(Game.Values.GameObject.Level2);
            }
            else if (SceneManager.GetActiveScene().name == Game.Values.GameObject.Level2)
            {
                SceneManager.LoadScene(Game.Values.GameObject.Level3);
            }
            else
            {
                GameObject.FindWithTag(Values.Tags.GameController).GetComponent<GameController>().OnGameEnd();
            }
        }
    }
}


