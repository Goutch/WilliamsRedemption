using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void HitSensorEventHandler(HitStimulus otherStimulus,IEntityData entityData);

public class HitSensor : MonoBehaviour
{
	public event HitSensorEventHandler OnHit;

	public void Hit(HitStimulus otherStimulus,IEntityData entityData)
	{
		NotifyHit(otherStimulus, entityData);
	}

	private void NotifyHit(HitStimulus otherStimulus,IEntityData entityData)
	{
		OnHit?.Invoke(otherStimulus,entityData);
	}
	
}
