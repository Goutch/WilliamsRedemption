using System.Collections;
using System.Collections.Generic;
using Game.Puzzle;
using Game.Puzzle.Light;
using UnityEngine;
using UnityEngine.Analytics;

public class LightMaster : MonoBehaviour
{

	[SerializeField] private ITriggerable[] lights;
	[SerializeField] private float timePerLight;
	private float timeAtStart;
	private float timeSinceStart;
	
	private int index;

	private void Awake()
	{
		index = 0;
		timeAtStart = 0;
		timeSinceStart = 0;
	}
	
	private void Update () {
		
		KeepTrackOfTime();
		
	}

	//TODO:: 
	private void Cycle()
	{
		
	}

	private void KeepTrackOfTime()
	{
		timeSinceStart = Time.time;
		if (timeAtStart == 0)
		{
			timeAtStart = Time.time;
		}
		
	}

	//TODO::
	private void ResetTime()
	{
		
	}
}
