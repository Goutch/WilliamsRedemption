using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour {



	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag(Values.Tags.Player))
		{
			GameObject.FindWithTag(Values.Tags.GameController).GetComponent<GameController>().OnGameEnd();
		}
	}
}
