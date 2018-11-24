using Game.Controller.Events;
using Game.Entity;
using Game.Entity.Player;
using Game.UI;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Controller
{
    public delegate void GameControllerEventHandler();

    public class GameController : MonoBehaviour
    {
        [SerializeField] private Level startLevel;
        [SerializeField] private AudioClip gameMusic;
        private int score;
        private int bonusScore;
        private float time;
        private int collectable;
        private float startTime;
        private int levelRemainingTime;
        private bool isGamePaused = false;
        private bool isGameStarted = false;
        private bool isGameInExpertMode = false;
        public bool ExpertMode => isGameInExpertMode;
        private bool isGameWinned = false;
        private bool spawnAtCheckPoint = false;

        private Level currentLevel;
        public event GameControllerEventHandler OnGameEnd;
        public event GameControllerEventHandler OnLevelChange;

        private CollectablesEventChannel collectablesEventChannel;

        private Checkpoint.CheckPointData currentCheckPointdata;

        private PlayerController player;

        //UI
        private ScoreUI scoreUI;
        private CollectablesUI collectableUI;
        private MenuManager menu;
        private LifePointsUI lifePointsUI;
        private LevelFinishedUI levelFinishUI;

        //Getters
        public AudioClip GameMusic => gameMusic;

        public float CurrentGameTime => time;

        public int LevelRemainingTime => levelRemainingTime;

        public int Score => score;

        public int TotalScore => score + bonusScore;

        public int BonusScore => bonusScore;

        public int CollectableAquiered => collectable;
        public bool IsGameStarted => isGameStarted;
        public bool IsGamePaused => isGamePaused;
        public bool IsGameWinned => isGameWinned;

        public int TotalTime => totalTime;

        private int totalTime = 0;

        public Level CurrentLevel => currentLevel;

        private void Awake()
        {
            menu = GetComponent<MenuManager>();
            collectableUI = GetComponent<CollectablesUI>();
            scoreUI = GetComponent<ScoreUI>();
            lifePointsUI = GetComponent<LifePointsUI>();
            levelFinishUI = GetComponent<LevelFinishedUI>();
            collectablesEventChannel = GetComponent<CollectablesEventChannel>();
            SceneManager.sceneLoaded += OnSceneLoaded;

            Scene[] loadedScenes = SceneManager.GetAllScenes();
            if (loadedScenes.Length == 1 && SceneManager.GetActiveScene().name == "Main")
            {
                SceneManager.LoadSceneAsync(Values.Scenes.Menu, LoadSceneMode.Additive);
            }
            else
            {
                foreach (var scene in loadedScenes)
                {
                    if (scene.name != Values.Scenes.Menu
                        || scene.name != Values.Scenes.Main)
                    {
                        currentLevel = startLevel;
                        menu.HideMainMenu();
                        menu.DisplayGameHUD();
                    }
                }
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != Values.Scenes.Menu && scene.name != Values.Scenes.Main)
            {
                startTime = Time.time;
                player = GameObject.FindGameObjectWithTag(Values.Tags.Player)
                    .GetComponent<PlayerController>();
                player.GetComponent<Health>().OnDeath += OnPlayerDie;
                if (!isGameInExpertMode && spawnAtCheckPoint)
                {
                    ReturnCheckPoint();
                }
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
                time = Time.time - startTime;
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
            if (ExpertMode)
                score *= 2;
            this.score += score;
            scoreUI.OnScoreChange();
        }

        private void ReturnCheckPoint()
        {
            spawnAtCheckPoint = false;
            score = currentCheckPointdata.ScoreAtTimeOfTrigger;
            collectable = 0;
            collectableUI.UpdateCollectableUI();
            scoreUI.OnScoreChange();
            Health playerHealth = player.GetComponent<Health>();
            playerHealth.ResetHealth();
            lifePointsUI.UpdateHealth();

            player.transform.position = currentCheckPointdata.PositionAtTimeOfTrigger;

            startTime = time - currentCheckPointdata.TimeAtTimeOfTrigger;
        }

        public void LevelFinished()
        {
            PauseGame();

            bonusScore += LevelRemainingTime;
            if (currentLevel.NextLevel != null)
            {
                levelFinishUI.OnLevelFinished();
                menu.DisplayLevelFinishedPanel();
            }
            else
            {
                Win();
                GameOver();
            }
        }

        public void NextLevel()
        {
            menu.HideLevelFinishedPanel();
            score += bonusScore;
            bonusScore = 0;
            scoreUI.OnScoreChange();


            OnLevelChange?.Invoke();
            if (SceneManager.GetActiveScene().name == Values.Scenes.Menu)
            {
                isGameStarted = true;
                isGamePaused = false;
                currentLevel = startLevel;
                LoadLevel(currentLevel);
            }
            else
            {
                player.GetComponent<Health>().OnDeath -= OnPlayerDie;
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
                    Win();
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
            bonusScore = 0;
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
            bonusScore = 0;
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
            isGameStarted = true;
            isGamePaused = false;
            isGameWinned = false;
            collectable = 0;
            menu.HidePausePanel();
            menu.HideMainMenu();
        }

        private void OnPlayerDie(GameObject receiver, GameObject attacker)
        {
            if (isGameInExpertMode)
                GameOver();
            else
            {
                spawnAtCheckPoint = true;
                Restart();
            }
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

        [UsedImplicitly]
        public void ActivateExpertMode()
        {
            isGameInExpertMode = true;
        }

        [UsedImplicitly]
        public void DesactivateExpertMode()
        {
            isGameInExpertMode = false;
        }

        public void OnCheckPointTrigerred(Checkpoint.CheckPointData checkpoint)
        {
            currentCheckPointdata = checkpoint;
        }

        public void AddCollectable(int scoreValue)
        {
            collectable++;
            AddScore(scoreValue);
            collectableUI.UpdateCollectableUI();
            collectablesEventChannel.Publish(new OnCollectableFound(currentLevel));
        }

        private void Win()
        {
            isGameWinned = true;
        }

        public void AddBonusScore(int bonus)
        {
            bonusScore += bonus;
        }
    }
}