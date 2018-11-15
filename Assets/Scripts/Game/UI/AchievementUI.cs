using Boo.Lang;
using Game.Controller;
using Unity.Collections.LowLevel.Unsafe;
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
        private AchievementManager achievementManager;

        private void OnEnable()
        {
            gameController = GameObject.FindGameObjectWithTag(Values.Tags.GameController)
                .GetComponent<GameController>();
            gameController.OnGameEnd += OnGameEnd;
            achievementManager = gameController.GetComponent<AchievementManager>();
        }

        private void OnGameEnd()
        {
            int bonusScoreValue = 0;

            string achievementListText = "";
            foreach (var achievement in achievementManager.AcomplishedAchievements)
            {
                achievementListText += achievement.name;
                achievementListText += ":";
                achievementListText += achievement.ScoreValue;
                achievementListText += "\n";
                bonusScoreValue += achievement.ScoreValue;
            }         
            
            achievementList.text = achievementListText;
            score.text = "Score:" + gameController.Score;
            bonus.text = "Bonus:" + bonusScoreValue;
            total.text = "Total:" + (gameController.Score + bonusScoreValue);
            
            gameController.AddScore(bonusScoreValue);
        }
    }
}