using Game.Controller;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class LevelFinishedUI : MonoBehaviour
    {
        [SerializeField] private Text scoreText;
        [SerializeField] private Text remainingTimeText;
        [SerializeField] private Text totalText;
        [SerializeField] private string scoreTextBegin = "Score:";
        [SerializeField] private string remainingTimeTextBegin = "Remaining time:";
        [SerializeField] private string totalTextBegin = "Total score:";
        private GameController gameController;

        private void Start()
        {
            gameController = GameObject.FindGameObjectWithTag(Values.GameObject.GameController)
                .GetComponent<GameController>();
        }

        public void OnLevelFinished()
        {
            scoreText.text = scoreTextBegin + gameController.Score;
            remainingTimeText.text = remainingTimeTextBegin + gameController.LevelRemainingTime;
            totalText.text = totalTextBegin + gameController.TotalScore;
        }
    }
}