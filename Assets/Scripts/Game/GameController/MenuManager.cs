using UnityEngine;
using UnityEngine.SceneManagement;

//BEN_REVIEW : Pourrait être une classe statique à la place, car ne fait qu'appeler des méthodes statiques. 
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
}
