using System.Collections;
using System.Collections.Generic;
using Harmony;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

	public void LoadLevel1()
	{
		SceneManager.LoadScene(/*R.S.Scene.Level1*/"Scenes/Level1");
	}

	public void ExitGame()
	{
		Application.Quit();
	}

	public void ReturnToMenu()
	{
		SceneManager.LoadScene(/*R.S.Scene.Menu*/"Scenes/Menu");
		Time.timeScale = 1.0f;
	}
}
