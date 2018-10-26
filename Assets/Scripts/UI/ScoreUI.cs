
using UnityEngine.UI;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
	private GameController gameController;
	private Text scoreText;
	private int score;

	private void Start()
	{
		gameController = GameObject.FindGameObjectWithTag(Values.GameObject.GAME_CONTROLLER).GetComponent<GameController>();
		gameController.OnScoreChange += OnScoreChange;
		scoreText = GameObject.Find(Values.GameObject.SCORE_TEXT).GetComponent<Text>();
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
