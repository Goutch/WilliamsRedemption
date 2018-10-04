using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void HealthEventHandler();
public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    public int MaxHealth => maxHealth;
	private int healthPoints;
	public event HealthEventHandler OnDeath;
    public event HealthEventHandler OnHealthChange;
	public int HealthPoints
	{
		get { return healthPoints; }
		set
        {
            healthPoints = value;
            OnHealthChange?.Invoke();

            if (healthPoints <= 0)
            {
                OnDeath?.Invoke();
                Destroy(this.transform.root.gameObject);
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
		HealthPoints = 0;
	}
}
