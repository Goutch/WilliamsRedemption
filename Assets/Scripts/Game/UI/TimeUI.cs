using Game.Controller;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class TimeUI : MonoBehaviour
    {

        [SerializeField] private int startingTime;
        public static TimeUI instance; //BEN_CORRECTION : Jamais utlisée.
        private GameController gameController;
        private Text timeText;
        private int time; //BEN_CORRECTION : Attribut inutile. Aurait pu être une variable dans une fonction. Me voir si pas clair.
        private int remainingTime; //BEN_CORRECTION : Attribut inutile. Aurait pu être une variable dans une fonction.
        private const string remainingTimeText = "Remaining Time : "; //BEN_CORRECTION : Aurait du être un "SerilizedField". Ça hérite de "MonoBehaviour", donc vous n'avez aucune excuse.

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
            //BEN_REVIEW : Ces deux fonction auraient pu être fusionnées en une seule.
            UpdateTimeValue();
            UpdateTimeText();
        }

        private void UpdateTimeValue()
        {
            if (!IsRemainingTimeOver())
            {
                //BEN_CORRECTION : Si vous avez déjà le "Time" dans "GameController", pourquoi est-ce que vous
                //                 effectuez toutes ces manipulations dessus ?
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

