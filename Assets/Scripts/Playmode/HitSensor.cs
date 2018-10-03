using Playmode.EnnemyRework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void HitSensorEventHandler(HitStimulus otherStimulus);

public class HitSensor : MonoBehaviour
{
	public event HitSensorEventHandler OnHit;
	public void Hit(HitStimulus otherStimulus)
	{
        NotifyHit(otherStimulus);
	}

	public void NotifyHit(HitStimulus otherStimulus)
	{
		OnHit?.Invoke(otherStimulus);
	}
}
