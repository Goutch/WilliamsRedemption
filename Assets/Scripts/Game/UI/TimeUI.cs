using Game.Controller;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class TimeUI : MonoBehaviour
    {
        [SerializeField] private Text timeText;
        [SerializeField] private string remainingTimeText = "Remaining Time : ";
        private GameController gameController;


        private void Start()
        {
            gameController = GameObject.FindGameObjectWithTag(Values.Tags.GameController)
                .GetComponent<GameController>();
        }

        private void Update()
        {
            timeText.text = remainingTimeText + Mathf.RoundToInt(gameController.LevelRemainingTime).ToString();
        }
    }
}