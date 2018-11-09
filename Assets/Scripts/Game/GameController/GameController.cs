using System.Collections.Generic;
using Game.Entity;
using Game.Entity.Player;
using Game.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.Controller
{
    public class GameController : MonoBehaviour
    {
        private int score;
        private float time;
        private int collectable;
        private int currentLevel;
        private float startTime;
        private bool isGamePaused = false;
        private bool isGameStarted = false;

        //UI
        private ScoreUI scoreUI;
        private CollectablesUI collectableUI;
        private PauseUI pauseUI;
        private MenuManager menu;

        public int CurrentLevel => currentLevel;
        public float CurrentGameTime => time;
        public int Score => score;
        public int CollectableAquiered => collectable;
        public bool IsGameStarted => isGameStarted;
        public bool IsGamePaused => isGamePaused;

        private AchievementEventChannel achievementEventChannel;

        void Start()
        {
            DontDestroyOnLoad(this.gameObject);
            menu = GetComponent<MenuManager>();
            achievementEventChannel = GetComponent<AchievementEventChannel>();
            pauseUI = GetComponent<PauseUI>();
            collectableUI = GetComponent<CollectablesUI>();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != Values.Scenes.Menu)
            {
                PlayerController.instance.GetComponent<Health>().OnDeath += OnPlayerDie;
                startTime = Time.time;
                score = 0;
            }
        }

        private void Update()
        {
            time = UnityEngine.Time.time - startTime;
        }

        public void AddScore(int score)
        {
            this.score += score;
        }

        public void AddCollectable(int scoreValue)
        {
            collectable++;
            AddScore(scoreValue);
            collectableUI.AddCollectable();
            achievementEventChannel.PublishPlayerFoundCollectable();
        }

        public void NextLevel()
        {
            if (SceneManager.GetActiveScene().name == Values.Scenes.Menu)
            {
                SceneManager.LoadScene(Values.Scenes.Level1);
            }
            else if (SceneManager.GetActiveScene().name == Values.Scenes.Level1)
            {
                SceneManager.LoadScene(Values.Scenes.Level2);
            }
            else if (SceneManager.GetActiveScene().name == Values.Scenes.Level2)
            {
                SceneManager.LoadScene(Values.Scenes.Level3);
            }
        }

        private void OnPlayerDie(GameObject gameObject)
        {
            menu.DisplayGameOverPanel();
        }

        public void PauseGame()
        {
            Debug.Log("Game paused");
            UnityEngine.Time.timeScale = 0f;
            isGamePaused = true;
        }

        public void ResumeGame()
        {
            Debug.Log("Game resumed");
            UnityEngine.Time.timeScale = 1f;
            isGamePaused = false;
        }
    }
}