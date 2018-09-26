using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{

	private Transform rootTransform;

	[SerializeField] protected float speed;
	[SerializeField] protected float distanceToGoFromSpawningPoint;
	
	public float Speed
	{
		get { return speed; }
		private set { speed = value; }
	}

	public float DistanceFromSpawningPoint
	{
		get { return distanceToGoFromSpawningPoint; }
		private set { distanceToGoFromSpawningPoint = value; }
	}

	void Awake () {
		InitializeComponent();
	}

	private void InitializeComponent()
	{
		rootTransform = transform.root;
	}

	public void MoveBat(int direction, ref Vector2 currentPosition)
	{
		rootTransform.Translate(new Vector2(speed*Time.deltaTime*direction,0));
		currentPosition = rootTransform.position;
	}
}
