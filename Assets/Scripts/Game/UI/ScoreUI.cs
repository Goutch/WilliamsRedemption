using Game.Controller;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] private Text scoreText;
        private GameController gameController;

        private void Start()
        {
            gameController = GameObject.FindGameObjectWithTag(Values.Tags.GameController)
                .GetComponent<GameController>();
            UpdateScoreText();
        }

        public void OnScoreChange()
        {
            UpdateScoreText();
        }


        private void UpdateScoreText()
        {
            scoreText.text = "Score : " + gameController.Score.ToString();
        }
    }
}