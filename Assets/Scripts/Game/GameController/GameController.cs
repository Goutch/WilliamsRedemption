using System.Collections.Generic;
using System.Linq;
using Game.Entity;
using Game.Entity.Player;
using Game.UI;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.Controller
{
    public delegate void GameControllerEventHandler();
        
    public class GameController : MonoBehaviour
    {

        
        private int score;
        private float time;
        private int collectable;
        private float startTime;
        private int levelRemainingTime;
        private bool isGamePaused = false;
        private bool isGameStarted = false;

        [SerializeField] private Level startLevel;
        private Level currentLevel;

        //UI
        private ScoreUI scoreUI;
        private CollectablesUI collectableUI;
        private MenuManager menu;
        [SerializeField] private LifePointsUI lifePointsUI;

        public float CurrentGameTime => time;

        public int LevelRemainingTime => levelRemainingTime;

        public int Score => score;

        public int CollectableAquiered => collectable;
        public bool IsGameStarted => isGameStarted;
        public bool IsGamePaused => isGamePaused;

        private AchievementEventChannel achievementEventChannel;
        public event GameControllerEventHandler OnGameEnd;

        void Start()
        {

            DontDestroyOnLoad(this.gameObject);
            menu = GetComponent<MenuManager>();
            achievementEventChannel = GetComponent<AchievementEventChannel>();
            collectableUI = GetComponent<CollectablesUI>();
            scoreUI = GetComponent<ScoreUI>();
            SceneManager.sceneLoaded += OnSceneLoaded;
  
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != Values.Scenes.Menu)
            {
                PlayerController.instance.GetComponent<Health>().OnDeath += OnPlayerDie;
                lifePointsUI.InitLifePoints();
                startTime = Time.time;
            }

            Time.timeScale = 1f;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!IsGamePaused)
                {
                    PauseGame();
                    menu.DisplayPausePanel();
                    
                }
                else
                {
                    ResumeGame();
                    menu.HidePausePanel();
                }
            }

            if (!isGamePaused && isGameStarted)
            {
                time = UnityEngine.Time.time - startTime;
                if (levelRemainingTime > 0)
                    levelRemainingTime = Mathf.RoundToInt((currentLevel.ExpectedTime - CurrentGameTime));
                else
                {
                    levelRemainingTime = 0;
                }
            }
        }

        public void AddScore(int score)
        {
            this.score += score;
            scoreUI.OnScoreChange();
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
                isGameStarted = true;
                isGamePaused = false;
                currentLevel = startLevel;
                LoadLevel(currentLevel);
            }
            else
            {
                PlayerController.instance.GetComponent<Health>().OnDeath -= OnPlayerDie;
                currentLevel = currentLevel.NextLevel;
                isGameStarted = true;
                isGamePaused = false;
                if (currentLevel != null)
                {
                    LoadLevel(currentLevel);
                }
                else
                {
                    GameOver();
                }
            }
        }

        public void GameOver()
        {
            menu.DisplayGameOverPanel();
            PauseGame();
            OnGameEnd?.Invoke();
        }

        [UsedImplicitly]
        public void ReturnMenu()
        {
            score = 0;
            currentLevel = null;
            isGameStarted = false;
            menu.ReturnToMenu();
        }

        [UsedImplicitly]
        public void Restart()
        {
            LoadLevel(currentLevel);
            menu.HideGameOverPanel();
            menu.HidePausePanel();
        }

        public void LoadLevel(Level level)
        {
            SceneManager.LoadScene(level.Scene.name);
            levelRemainingTime = level.ExpectedTime;
            score = 0;
            startTime = Time.time;
            time = 0;
            isGameStarted = true;
            isGamePaused = false;
        }

        private void OnPlayerDie(GameObject gameObject)
        {
            GameOver();
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