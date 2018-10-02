using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void HitSensorEventHandler();

public class HitSensor : MonoBehaviour
{
	public event HitSensorEventHandler OnHit;

	public void Hit()
	{
        Debug.Log("Hit");
		NotifyHit();
	}

	private void NotifyHit()
	{
		OnHit?.Invoke();
	}
	
}
