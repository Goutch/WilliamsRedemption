
using Harmony;
using UnityEngine.UI;
using UnityEngine;

public class TimeUI : MonoBehaviour
{

	[SerializeField] private int startingTime;
	public static TimeUI instance;
	private GameController gameController;
	private Text timeText;
	private int time;
	private int remainingTime;
	private const string remainingTimeText = "Remaining Time : ";

	private void Start()
	{
		gameController = GameObject.FindGameObjectWithTag(Values.GameObject.GAME_CONTROLLER).GetComponent<GameController>();
		timeText = GameObject.Find(Values.GameObject.TIME_TEXT).GetComponent<Text>();
		gameController.OnTimeChange += OnTimeChange;
		remainingTime = startingTime;
		OnTimeChange();
	}

	private void OnTimeChange()
	{
		UpdateTimeValue();
		UpdateTimeText();
	}

	private void UpdateTimeValue()
	{
		if (!IsRemainingTimeOver())
		{
			time = Mathf.RoundToInt(gameController.Time);
			remainingTime = startingTime - time;
		}	
	}
	private void UpdateTimeText()
	{
		timeText.text = remainingTimeText + remainingTime.ToString();
	}

	public bool IsRemainingTimeOver()
	{
		return remainingTime <= 0;
	}
}
