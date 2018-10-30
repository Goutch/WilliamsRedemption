using Game.Controller;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class TimeUI : MonoBehaviour
    {

        [SerializeField] private int startingTime;
        public static TimeUI instance;
        private GameController gameController;
        private Text timeText;
        private int time;
        private int remainingTime;
        private const string remainingTimeText = "Remaining Time : ";

        private void Start()
        {
            gameController = GameObject.FindGameObjectWithTag(Values.Tags.GameController).GetComponent<GameController>();
            timeText = GameObject.Find(Values.GameObject.TimeText).GetComponent<Text>();
            gameController.OnTimeChange += OnTimeChange;
            remainingTime = startingTime;
            OnTimeChange();
        }

        private void OnTimeChange()
        {
            UpdateTimeValue();
            UpdateTimeText();
        }

        private void UpdateTimeValue()
        {
            if (!IsRemainingTimeOver())
            {
                time = Mathf.RoundToInt(gameController.Time);
                remainingTime = startingTime - time;
            }
        }
        private void UpdateTimeText()
        {
            timeText.text = remainingTimeText + remainingTime.ToString();
        }

        public bool IsRemainingTimeOver()
        {
            return remainingTime <= 0;
        }
    }

}

