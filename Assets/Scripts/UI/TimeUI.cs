
using Harmony;
using UnityEngine.UI;
using UnityEngine;

public class TimeUI : MonoBehaviour
{

	public static TimeUI instance;
	private GameController gameController;
	private Text timeText;
	private double time;
	private const double STARTING_TIME = 300;
	private double remainingTime;

	private void Start()
	{
		gameController = GameObject.FindGameObjectWithTag(/*R.S.GameObject.GameController*/"GameController").GetComponent<GameController>();
		timeText = GameObject.Find("GameController/Canvas/TimeText").GetComponent<Text>();
		gameController.OnTimeChange += OnTimeChange;
		remainingTime = STARTING_TIME;
		OnTimeChange();
	}

	private void OnTimeChange()
	{
		UpdateTimeValue();
		UpdateTimeText();
	}

	private void UpdateTimeValue()
	{
		time = System.Math.Round(gameController.Time,2);
		remainingTime = STARTING_TIME - time;
	}
	private void UpdateTimeText()
	{
		timeText.text = "Remaining Time : " + remainingTime.ToString();
	}

	public bool IsRemainingTimeOver()
	{
		return remainingTime <= 0;
	}
}
