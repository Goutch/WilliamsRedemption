using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{

	private Transform rootTransform;

	[SerializeField] protected float speed;
	
	public float Speed
	{
		get { return speed; }
		private set { speed = value; }
	}

	private void Awake () {
		InitializeComponent();
	}

	private void InitializeComponent()
	{
		rootTransform = transform.root;
	}

	public void MoveBat(int direction)
	{
		rootTransform.Translate(new Vector2(speed*Time.deltaTime*direction,0));		
	}
}
