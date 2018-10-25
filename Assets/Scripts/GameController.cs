using System.Collections;
using System.Collections.Generic;
using Game.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public delegate void ScoreEventHandler();

public delegate void TimeEventHandler();

public class GameController : MonoBehaviour
{
	private int score;
	private float time;
	private float startTime;
	public event ScoreEventHandler OnScoreChange;
	public event TimeEventHandler OnTimeChange;
	private PauseUI pauseUI;
	private Text deathText;
	private CollectablesUI collectableUI;
	private const string deathTextString = "Game Over";
	public int Score => score;
	public float Time => time;

	void Start ()
	{
		PlayerController.instance.GetComponent<Health>().OnDeath += OnPlayerDie;
		startTime = UnityEngine.Time.time;
		pauseUI = GetComponent<PauseUI>();
		collectableUI = GetComponent<CollectablesUI>();
	}

	private void Update()
	{
		UpdateTime();
		UpdatePause();
	}
	
	public void AddScore(int score)
	{
		this.score += score;
		OnScoreChange?.Invoke();
	}
	private void OnPlayerDie(GameObject gameObject)
	{
		ShowDeathMenu();
	}
	private void UpdateTime()
	{
		time = UnityEngine.Time.time-startTime;
		OnTimeChange?.Invoke();
	}

	public void AddCollectable(int scoreValue)
	{
		AddScore(scoreValue);
		collectableUI.AddCollectable();
		
	}
	private void UpdatePause()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			pauseUI.OnPressKeyPause();
		}
	}

	private void ShowDeathMenu()
	{
		GameObject.Find(/*R.S.GameObject.PanelPause*/"GameController/Canvas/PanelPause").SetActive(true);
		GameObject.Find(/*R.S.GameObject.ButtonRestartGame*/"GameController/Canvas/PanelPause/ButtonRestartGame").SetActive(true);
		deathText = GameObject.Find("GameController/Canvas/PanelPause/Text").GetComponent<Text>();
		deathText.text = deathTextString;
	}
}

