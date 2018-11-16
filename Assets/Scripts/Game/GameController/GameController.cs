﻿using System.Collections.Generic;
using System.Linq;
using Game.Controller.Events;
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
        [SerializeField] private Level startLevel;
        private int score;
        private float time;
        private int collectable;
        private float startTime;
        private int levelRemainingTime;
        private bool isGamePaused = false;
        private bool isGameStarted = false;
        private Level currentLevel;
        public event GameControllerEventHandler OnGameEnd;

        private CollectablesEventChannel collectablesEventChannel;
        //UI
        private ScoreUI scoreUI;
        private CollectablesUI collectableUI;
        private MenuManager menu;
        private LifePointsUI lifePointsUI;

        //Getters
        public float CurrentGameTime => time;

        public int LevelRemainingTime => levelRemainingTime;

        public int Score => score;

        public int CollectableAquiered => collectable;
        public bool IsGameStarted => isGameStarted;
        public bool IsGamePaused => isGamePaused;

        public int TotalTime => totalTime;

        private int totalTime = 0;

        void Start()
        {
            menu = GetComponent<MenuManager>();
            collectableUI = GetComponent<CollectablesUI>();
            scoreUI = GetComponent<ScoreUI>();
            lifePointsUI = GetComponent<LifePointsUI>();
            collectablesEventChannel = GetComponent<CollectablesEventChannel>();
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadSceneAsync(Values.Scenes.Menu, LoadSceneMode.Additive);
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
            SceneManager.SetActiveScene(scene);
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
            collectablesEventChannel.Publish(new OnCollectableFound(currentLevel));
        }

        public void NextLevel()
        {
            if (SceneManager.GetActiveScene().name== Values.Scenes.Menu)
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
                totalTime += Mathf.RoundToInt(CurrentGameTime);
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
            PauseGame();
            OnGameEnd?.Invoke();
            menu.DisplayGameOverPanel();
        }

        [UsedImplicitly]
        public void ReturnMenu()
        {
            currentLevel = null;
            isGameStarted = false;
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
            SceneManager.LoadSceneAsync(Game.Values.Scenes.Menu, LoadSceneMode.Additive);
            menu.ReturnToMenu();
            score = 0;
            scoreUI.OnScoreChange();
            totalTime = 0;
            collectable = 0;
        }

        [UsedImplicitly]
        public void Restart()
        {
            LoadLevel(currentLevel);
            menu.HideGameOverPanel();
            menu.HidePausePanel();
            score = 0;
            scoreUI.OnScoreChange();
            totalTime = 0;
            collectable = 0;
        }

        public void LoadLevel(Level level)
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
            SceneManager.LoadSceneAsync(level.Scene.name, LoadSceneMode.Additive);
            levelRemainingTime = level.ExpectedTime;
            startTime = Time.time;
            time = 0;
            isGameStarted = true;
            isGamePaused = false;
            collectable = 0;
            menu.HidePausePanel();
            menu.HideMainMenu();
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