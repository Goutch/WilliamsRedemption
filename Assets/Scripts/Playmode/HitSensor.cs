using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void HitSensorEventHandler(int hitPoints);

public class HitSensor : MonoBehaviour
{
	public event HitSensorEventHandler OnHit;

	public void Hit(int hitPoints)
	{
		NotifyHit(hitPoints);
	}

	private void NotifyHit(int hitPoints)
	{
		OnHit?.Invoke(hitPoints);
	}
}
