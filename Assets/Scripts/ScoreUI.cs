
using UnityEngine.UI;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{

	public static ScoreUI instance;
	private GameController gameController;
	public Text scoreText;
	private int score;

	private void Start()
	{
		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		gameController.OnScoreChange += OnScoreChange;
		score = gameController.Score;
		UpdateScoreText();
	}

	private void OnScoreChange()
	{
		UpdateScoreValue();
		UpdateScoreText();
	}

	private void UpdateScoreValue()
	{
		score = gameController.Score;
	}
	private void UpdateScoreText()
	{
		scoreText.text = "Score : " + score.ToString();
	}
}
