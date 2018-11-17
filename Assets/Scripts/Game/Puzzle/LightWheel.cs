using System.Collections;
using System.Collections.Generic;
using Game.Puzzle.Light;
using Math;
using UnityEngine;

public class LightWheel : MonoBehaviour
{
	[SerializeField] private List<CircleLight> lights = new List<CircleLight>(4);
	[SerializeField] private float rotatingSpeed;

	// Use this for initialization
	private void Awake()
	{
	}


	
	// Update is called once per frame
	private void Update ()
	{
		RotateWheel();
	}
	private void RotateWheel()
	{	
		foreach (var l in lights)
		{
			l.FaceAngle += rotatingSpeed*Time.deltaTime;
		}
	}
}
