using System.Net.Configuration;
using Game.Controller;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    //BEN_CORRECTION : Nommage.
    [SerializeField] private GameObject PauseMenuPanel;
    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gameHUD;
    [SerializeField] private GameObject levelFinishedPanel;
    private GameController gameController;
    
    //BEN_CORRECTION : Ces deux constantes auraient du être des "SerializedFields".
    private const string gameCompletedTextString = "Congratulations!";
    private const string deathTextString = "Game Over";


    private void Awake()
    {
        gameController = GetComponent<GameController>();
        SceneManager.sceneLoaded += OnSceneLoaded;
        HideGameOverPanel();
        HidePausePanel();
        HideGameHUD();
        HideLevelFinishedPanel();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //BEN_CORRECTION : La scène "Menu" me semble pas mal inutile dans l'état actuel.
        //                 Je crois que, coté conception, ce serait à revoir. Par contre,
        //                 à cause que le projet est assez avancé, j'y retoucherait que
        //                 si un bogue important est trouvé en lien avec tout ça.
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
        HideGameOverPanel();
        HidePausePanel();
        HideGameHUD();
        DisplayMainMenu();
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

    public void DisplayLevelFinishedPanel()
    {
        levelFinishedPanel.SetActive(true);
    }

    public void HideLevelFinishedPanel()
    {
        levelFinishedPanel.SetActive(false);
    }
}