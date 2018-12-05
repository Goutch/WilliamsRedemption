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
        [SerializeField] private Level[] levels = new Level[3];
        private int score;
        private int bonusScore;
        private int collectable;
        private bool isGamePaused = false;
        private bool isGameStarted = false;
        private bool isGameInExpertMode = false;
        public bool ExpertMode => isGameInExpertMode;
        private bool isGameWinned = false;
        private bool spawnAtCheckPoint = false;
        private AchievementManager achievementManager;

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
        private EventTimerUI eventTimerUI;

        //Getters

        public int Score => score;

        public int TotalScore => score + bonusScore;

        public int BonusScore => bonusScore;

        public int CollectableAquiered => collectable;
        public bool IsGameStarted => isGameStarted;
        public bool IsGamePaused => isGamePaused;
        public bool IsGameWinned => isGameWinned;

        public Level CurrentLevel => currentLevel;

        //Time access
        public int TotalTime { get; set; }
        public int LevelRemainingTime { get; set; }
        private float savedTime;
        private float actualTimeSaved;
        
        public float EventTime { get; set; }

        private float startTime;

        private void Awake()
        {
            menu = GetComponent<MenuManager>();
            collectableUI = GetComponent<CollectablesUI>();
            scoreUI = GetComponent<ScoreUI>();
            lifePointsUI = GetComponent<LifePointsUI>();
            levelFinishUI = GetComponent<LevelFinishedUI>();
            eventTimerUI = GetComponent<EventTimerUI>();
            collectablesEventChannel = GetComponent<CollectablesEventChannel>();
            SceneManager.sceneLoaded += OnSceneLoaded;
            achievementManager = GetComponent<AchievementManager>();
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
                LevelRemainingTime = Mathf.RoundToInt(currentLevel.ExpectedTime - (Time.time - startTime) - actualTimeSaved);
                if (LevelRemainingTime < 0)
                    LevelRemainingTime = 0;
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
            if (currentCheckPointdata.DoorsToOpenOnRespawn.Count > 0)
            {
                foreach (var door in currentCheckPointdata.DoorsToOpenOnRespawn)
                {
                    door.Unlock();
                }
            }
            spawnAtCheckPoint = false;
            score = currentCheckPointdata.ScoreAtTimeOfTrigger;
            collectable = 0;
            collectableUI.UpdateCollectableUI();
            scoreUI.OnScoreChange();
            Health playerHealth = player.GetComponent<Health>();
            playerHealth.ResetHealth();
            lifePointsUI.UpdateHealth();
            player.transform.position = currentCheckPointdata.PositionAtTimeOfTrigger;
        }

        public void LevelFinished()
        {
            PauseGame();

            bonusScore += 2 * LevelRemainingTime;
            if (currentLevel.NextLevel != null)
            {
                TotalTime += LevelRemainingTime;
                levelFinishUI.OnLevelFinished();
                menu.DisplayLevelFinishedPanel();
            }
            else
            {
                Win();
                GameOver();
            }
        }

        public void ChangeStartLevel(int level)
        {
            if (level == 0)
            {
                startLevel = levels[0];
            }
            else if (level == 1)
            {
                startLevel = levels[1];
            }
            else if (level == 2)
            {
                startLevel = levels[2];
            }
        }

        public void NextLevel()
        {
            menu.HideLevelFinishedPanel();
            score += bonusScore;
            bonusScore = 0;
            collectable = 0;
            scoreUI.OnScoreChange();


            OnLevelChange?.Invoke();
            if (SceneManager.GetActiveScene().name == Values.Scenes.Menu)
            {
                isGameStarted = true;
                isGamePaused = false;
                if (isGameInExpertMode)
                {
                    startLevel = levels[0];
                }
                currentLevel = startLevel;
                
                LoadLevel(currentLevel);
            }
            else
            {
                player.GetComponent<Health>().OnDeath -= OnPlayerDie;
                currentLevel = currentLevel.NextLevel;
                isGameStarted = true;
                isGamePaused = false;
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
            TotalTime += LevelRemainingTime;

            PauseGame();
            OnGameEnd?.Invoke();
            menu.DisplayGameOverPanel();
        }

        [UsedImplicitly]
        public void ReturnMenu()
        {
            currentLevel = null;
            isGameStarted = false;
            achievementManager.Reset();
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
            SceneManager.LoadSceneAsync(Game.Values.Scenes.Menu, LoadSceneMode.Additive);
            menu.ReturnToMenu();
            score = 0;
            bonusScore = 0;
            scoreUI.OnScoreChange();
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
            collectable = 0;
        }

        public void LoadLevel(Level level)
        {
            startTime = Time.time;
            actualTimeSaved = savedTime;
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
            SceneManager.LoadSceneAsync(level.Scene, LoadSceneMode.Additive);
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
            savedTime = currentLevel.ExpectedTime - LevelRemainingTime;
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