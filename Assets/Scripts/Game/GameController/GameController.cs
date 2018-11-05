using Game.Entity;
using Game.Entity.Player;
using Game.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Controller
{
    public delegate void ScoreEventHandler();

    public delegate void TimeEventHandler();

    public class GameController : MonoBehaviour
    {
        private int score;
        private float time;
        private float startTime;
        public event ScoreEventHandler OnScoreChange;
        public event TimeEventHandler OnTimeChange;
        //BEN_CORRECTION : GameController ne devrait pas directement utiliser "PauseUI".
        //                 GameController peut savoir si le jeu est en pause ou non, mais il ne devrait
        //                 pas directement utiliser "PauseUI".
        //
        //                 Pour que "PauseUI" sache que le jeu est en pause, utilisez un "Event", tout comme vous
        //                 avec fait pour le "Score" et le "Time".
        private PauseUI pauseUI; 
        private Text deathText;
        private CollectablesUI collectableUI;
        private const string deathTextString = "Game Over"; 
        public int Score => score;
        public float Time => time;

        void Start()
        {
            PlayerController.instance.GetComponent<Health>().OnDeath += OnPlayerDie;
            startTime = UnityEngine.Time.time;
            pauseUI = GetComponent<PauseUI>();
            collectableUI = GetComponent<CollectablesUI>();
        }

        private void Update()
        {
            UpdateTime();
            UpdatePause();
        }

        public void AddScore(int score)
        {
            this.score += score;
            OnScoreChange?.Invoke();
        }
        private void OnPlayerDie(GameObject gameObject)
        {
            ShowDeathMenu();
        }
        private void UpdateTime()
        {
            time = UnityEngine.Time.time - startTime;
            OnTimeChange?.Invoke();
        }

        public void AddCollectable(int scoreValue)
        {
            AddScore(scoreValue);
            //BEN_CORRECTION : Le compte de "Collectibles" devrait être soit dans GameController, soit dans une
            //                 autre classe. Il ne devrait pas être dans le UI.
            collectableUI.AddCollectable();

        }
        private void UpdatePause()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pauseUI.OnPressKeyPause();
            }
        }

        public void OnGameEnd()
        {
            pauseUI.OnPressKeyPause();
        }
        private void ShowDeathMenu()
        {
            pauseUI.OnPressKeyPause();
            //BEN_CORRECTION : Logique réservée au UI. Devrait être dans "PauseUI".
            GameObject.Find(Values.GameObject.ButtonRestartGame).SetActive(true);
            deathText = GameObject.Find(Values.GameObject.TextPause).GetComponent<Text>();
            deathText.text = deathTextString;
        }
    }
}
