using System.Net.Configuration;
using Game.Controller;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenuPanel;
    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gameHUD;
    private GameController gameController;
    private const string gameCompletedTextString = "Congratulations!";
    private const string deathTextString = "Game Over";


    private void Start()
    {
        gameController = GetComponent<GameController>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == Game.Values.Scenes.Menu)
        {
            DisplayMainMenu();
            HideGameHUD();
        }
        else
        {
            HideMainMenu();
            DisplayGameHUD();
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(Game.Values.Scenes.Menu);
        Time.timeScale = 1.0f;
    }

    public void OnStartButtonClick()
    {
        gameController.NextLevel();
    }

    public void HideMainMenu()
    {
        mainMenuPanel.SetActive(false);
    }

    public void DisplayMainMenu()
    {
        mainMenuPanel.SetActive(true);
    }

    public void DisplayGameOverPanel()
    {
        GameOverPanel.SetActive(true);
    }

    public void HideGameOverPanel()
    {
        GameOverPanel.SetActive(false);
    }

    public void DisplayPausePanel()
    {
       PauseMenuPanel.SetActive(true);
    }

    public void HidePausePanel()
    {
        PauseMenuPanel.SetActive(false);
    }

    public void DisplayGameHUD()
    {
        gameHUD.SetActive(true);
    }

    public void HideGameHUD()
    {
        
        gameHUD.SetActive(false);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!gameController.IsGamePaused)
            {
                DisplayPausePanel();
                gameController.PauseGame();
            }
            else
            {
                HidePausePanel();
                gameController.ResumeGame();
            }
        }
    }
}