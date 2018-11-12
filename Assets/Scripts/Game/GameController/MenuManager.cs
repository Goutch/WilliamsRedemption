using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

	public void LoadLevel1()
	{
		SceneManager.LoadScene(Game.Values.GameObject.Level1);
		Time.timeScale = 1.0f;
	}

	public void ExitGame()
	{
		Application.Quit();
	}

	public void ReturnToMenu()
	{
		SceneManager.LoadScene(Game.Values.GameObject.Menu);
		Time.timeScale = 1.0f;
	}

	public void GoToLeaderboardOnWeb()
	{
		Application.OpenURL("http://35.188.160.44/leaderboardPage");
	}
}
