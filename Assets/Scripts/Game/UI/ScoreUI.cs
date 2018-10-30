
using Game.Controller;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ScoreUI : MonoBehaviour
    {
        private GameController gameController;
        private Text scoreText;
        private int score;

        private void Start()
        {
            gameController = GameObject.FindGameObjectWithTag(Values.Tags.GameController).GetComponent<GameController>();
            gameController.OnScoreChange += OnScoreChange;
            scoreText = GameObject.Find(Values.GameObject.ScoreText).GetComponent<Text>();
            score = gameController.Score;
            UpdateScoreText();
        }

        private void OnScoreChange()
        {
            UpdateScoreValue();
            UpdateScoreText();
        }

        private void UpdateScoreValue()
        {
            score = gameController.Score;
        }
        private void UpdateScoreText()
        {
            scoreText.text = "Score : " + score.ToString();
        }
    }
}


