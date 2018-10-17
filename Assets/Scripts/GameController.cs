using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public delegate void ScoreEventHandler();
public class GameController : MonoBehaviour
{
	private int score=0;
	private float time = 0;
	public event ScoreEventHandler OnScoreChange;

	public int Score => score;

	private void Start ()
	{
		PlayerController.instance.GetComponent<Health>().OnDeath += OnPlayerDie;
	}

	public void AddScore(int score)
	{
		this.score += score;
		OnScoreChange?.Invoke();
	}
	private void OnPlayerDie(GameObject gameObject)
	{
		SceneManager.LoadScene("Level" + 1);
	}
}
