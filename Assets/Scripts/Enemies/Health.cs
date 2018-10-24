﻿using System;
using System.Collections;
using System.Collections.Generic;
using Playmode.EnnemyRework;
using UnityEngine;

public delegate void HealthEventHandler(GameObject gameObject);
public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth;
	private bool isKilledByPlayer = true;
    public int MaxHealth => maxHealth;
	private int healthPoints;
	public event HealthEventHandler OnDeath;
    public event HealthEventHandler OnHealthChange;
	private bool isSupposedToBeDead = false;

	public int HealthPoints
	{
		get { return healthPoints; }
		private set
        {
            healthPoints = value;
            OnHealthChange?.Invoke(gameObject);

	        /*if (IsAnEnemy() && IsDead())
	        {
		        Debug.Log(Time.time);
	        }*/
            if (IsDead() /*&& !isSupposedToBeDead*/)
            {
	            Debug.Log(GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().Score);
	            if (isKilledByPlayer && IsAnEnemy())
	            {
		            AddEnemyScoreToGameScore();
	            }

	            isSupposedToBeDead = true;
	            OnDeath?.Invoke(transform.root.gameObject);
                //Destroy(this.transform.root.gameObject);
            }
        }
	}

	void Awake () {
        HealthPoints = MaxHealth;
	}
	
	public void Hit()
	{
		HealthPoints -= 1;
	}

	public void Kill()
	{
		isKilledByPlayer = false;
		HealthPoints = 0;
	}

	private bool IsDead()
	{
		return healthPoints <= 0;
	}

	private void AddEnemyScoreToGameScore()
	{
		int score = GetComponent<Enemy>().ScoreValue;
		GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().AddScore(score);
	}

	private bool IsAnEnemy()
	{
		return GetComponent<Enemy>() != null;
	}
}
