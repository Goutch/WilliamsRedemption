using System.Collections;
using System.Collections.Generic;
using Game.Controller;
using UnityEngine;
using UnityEngine.UI;


namespace Game.UI
{	
	public class EventTimerUI : MonoBehaviour
	{
		
		[SerializeField] private Text timeText;
		private GameController gameController;

		// Use this for initialization
		void Start()
		{
			gameController = GameObject.FindGameObjectWithTag(Values.Tags.GameController)
				.GetComponent<GameController>();
		}

		private void Update()
		{
			if (gameController.EventTime > 0)
			{
				timeText.text = Mathf.RoundToInt(gameController.EventTime).ToString();
			}
			else
			{
				timeText.text = null;
			}
			
		}


	}
}
