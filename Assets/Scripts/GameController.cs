using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	private int score=0;

	private float time = 0;

	// Use this for initialization
	void Start ()
	{
		PlayerController.instance.GetComponent<Health>().OnDeath += OnPlayerDie;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnPlayerDie()
	{
		SceneManager.LoadScene("Level" + 1);
	}
}
