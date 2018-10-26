using System.Collections;
using System.Collections.Generic;
using Harmony;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

	public void LoadLevel1()
	{
		SceneManager.LoadScene(Values.GameObject.LEVEL1);
		Time.timeScale = 1.0f;
	}

	public void ExitGame()
	{
		Application.Quit();
	}

	public void ReturnToMenu()
	{
		SceneManager.LoadScene(Values.GameObject.MENU);
		Time.timeScale = 1.0f;
	}
}
