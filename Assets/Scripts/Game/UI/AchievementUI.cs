using System.Xml.XPath;
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
            achievementManager = gameController.GetComponent<AchievementManager>();
        }

        public void DisplayAchievements(string achievementListText)
        {
           
            achievementList.text = achievementListText;
            score.text = "Score:" + gameController.Score;
            bonus.text = "Bonus:" + gameController.BonusScore;
            total.text = "Total:" + (gameController.TotalScore);
            

        }
    }
}