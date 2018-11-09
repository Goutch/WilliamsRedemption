using Game.Controller;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] private Text scoreText;
        private GameController gameController;
        private int score;

        private void Start()
        {
            gameController = GameObject.FindGameObjectWithTag(Values.Tags.GameController)
                .GetComponent<GameController>();
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