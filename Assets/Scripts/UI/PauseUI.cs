using System.Collections;
using System.Collections.Generic;
using Harmony;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{

	private bool isGamePaused = false;
	
	public void OnPressKeyPause()
	{
		if (isGamePaused)
		{
			isGamePaused = false;
			Time.timeScale = 1.0f;
			GameObject.Find(/*R.S.GameObject.PanelPause*/"GameController/Canvas/PanelPause").SetActive(false);
		}
		else
		{
			isGamePaused = true;
			Time.timeScale = 0f;
			GameObject.Find(/*R.S.GameObject.PanelPause*/"GameController/Canvas/PanelPause").SetActive(true);
		}	
	}
}
