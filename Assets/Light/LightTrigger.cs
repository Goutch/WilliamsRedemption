using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void LightEventHandler();
public class LightTrigger : MonoBehaviour {
	private static LightTrigger _instance;

	public static LightTrigger Instance { get { return _instance; } }
	
	public event LightEventHandler InLight;

	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
		} else {
			_instance = this;
		}
	}

	public void NotifyInLight()
	{
		if(InLight != null) InLight();
	}
}
