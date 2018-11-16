using System.Collections;
using System.Collections.Generic;
using Game.Puzzle.Light;
using Math;
using UnityEngine;

public class LightWheel : MonoBehaviour
{

	[SerializeField] private CircleLight[] lights = new CircleLight[4];
	[SerializeField] private List<CircleLight> ligghts = new List<CircleLight>(4);
	[SerializeField] private float rotatingSpeed;
	private float angleR;
	private BoxCollider2D box;

	// Use this for initialization
	private void Awake()
	{
		box = GetComponent<BoxCollider2D>();
		angleR = 0;
	}

	private void Start()
	{
		/*float angle = 0;
		for (int i = 0; i < lights.Length;i++)
		{
			lights[i].FaceAngle = angle;
			angle += 90;
			if (i == 1)
			{
				lights[i].transform.position = new Vector3(box.transform.position.x,box.transform.position.y+box.size.y/2);
			}
			else if (i == 3)
			{
				lights[i].transform.position = new Vector3(box.transform.position.x,box.transform.position.y -box.size.y/2);
			}
			else if(i ==0)
			{
				lights[i].transform.position = new Vector3(box.transform.position.x+box.size.x/2,box.transform.position.y);
			}
			else
			{
				lights[i].transform.position = new Vector3(box.transform.position.x-box.size.x/2,box.transform.position.y);
			}
				
		}*/
	}
	
	// Update is called once per frame
	private void Update ()
	{
		angleR += rotatingSpeed * Time.deltaTime;
		RotateWheel();
	}
	private void RotateWheel()
	{			
		transform.Rotate(0,0,angleR);
	}
}
