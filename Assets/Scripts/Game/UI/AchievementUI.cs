using Game.Controller;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class AchievementUI : MonoBehaviour
    {
        [SerializeField] private Text achievementList;
        [SerializeField] private Text score;
        [SerializeField] private Text bonus;
        [SerializeField] private Text total;
        private GameController gameController;

        private void OnEnable()
        {
            gameController = GameObject.FindGameObjectWithTag(Values.Tags.GameController)
                .GetComponent<GameController>();
            gameController.OnGameEnd += OnGameEnd;
        }

        private void OnGameEnd()
        {
            int bonusScoreValue = 3;
            score.text = "Score:" + gameController.Score;


            bonus.text = "Bonus:" + bonusScoreValue;
            total.text = "Total:" + (gameController.Score + bonusScoreValue);
        }
    }
}