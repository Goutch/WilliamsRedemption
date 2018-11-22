using Game.Controller.Events;
using Game.Entity;
using Game.Entity.Player;
using Game.UI;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Controller
{
    public delegate void GameControllerEventHandler();

    public class GameController : MonoBehaviour
    {
        [SerializeField] private Level startLevel;
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

        private Level currentLevel;
        public event GameControllerEventHandler OnGameEnd;
        public event GameControllerEventHandler OnLevelChange;

        private CollectablesEventChannel collectablesEventChannel;

        private Checkpoint currentCheckPoint;

        private PlayerController player;

        //UI
        private ScoreUI scoreUI;
        private CollectablesUI collectableUI;
        private MenuManager menu;
        private LifePointsUI lifePointsUI;
        private LevelFinishedUI levelFinishUI;

        //Getters
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

        void Start()
        {
            menu = GetComponent<MenuManager>();
            collectableUI = GetComponent<CollectablesUI>();
            scoreUI = GetComponent<ScoreUI>();
            lifePointsUI = GetComponent<LifePointsUI>();
            levelFinishUI = GetComponent<LevelFinishedUI>();
            collectablesEventChannel = GetComponent<CollectablesEventChannel>();
            SceneManager.sceneLoaded += OnSceneLoaded;
            if (SceneManager.GetActiveScene().name == "Main")
                SceneManager.LoadSceneAsync(Values.Scenes.Menu, LoadSceneMode.Additive);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != Values.Scenes.Menu)
            {
                lifePointsUI.InitLifePoints();
                startTime = Time.time;
                player = GameObject.FindGameObjectWithTag(Values.Tags.Player)
                    .GetComponent<PlayerController>();
                player.GetComponent<Health>().OnDeath += OnPlayerDie;
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

        public void LevelFinished()
        {
            PauseGame();
            menu.DisplayLevelFinishedPanel();
            bonusScore += LevelRemainingTime;
            if (currentLevel.NextLevel != null)
            {
                levelFinishUI.OnLevelFinished();
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
            time = 0;
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
                if (currentCheckPoint != null)
                {
                    Health playerHealth = player.GetComponent<Health>();
                    playerHealth.ResetHealth();

                    player.transform.position = currentCheckPoint.transform.position;

                    score = currentCheckPoint.ScoreAtTimeOfTrigger - (collectable * 100);
                    collectable = 0;
                    startTime = currentCheckPoint.TimeAtTimeOfTrigger + time;
                }
                else
                {
                    GameOver();
                }
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

        public void OnCheckPointTrigerred(Checkpoint checkpoint)
        {
            currentCheckPoint = checkpoint;
        }

        public void AddCollectable(int scoreValue)
        {
            collectable++;
            AddScore(scoreValue);
            collectableUI.AddCollectable();
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