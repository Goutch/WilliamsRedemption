using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	protected Health health;
	protected MovementManager movementManager;
	protected static Vector2 startingPosition;
	protected Vector2 actualPosition;
	private HitSensor hitSensor;
	protected int direction;

	void Awake () {
		InitializeComponent();
	}

	private void InitializeComponent()
	{
		startingPosition = transform.position;
		actualPosition = transform.position;
		direction = 1;
		health = GetComponent<Health>();
		hitSensor = transform.root.GetComponentInChildren<HitSensor>();
		movementManager = GetComponent<MovementManager>();
	}

	private void OnEnable()
	{
		hitSensor.OnHit += OnHit;
	}

	private void OnHit(int hitPoints)
	{
		health.Hit(hitPoints);
		Debug.Log(health.HealthPoints);
		OnHealthChange(health.HealthPoints);
	}

	private void OnHealthChange(int newHealth)
	{
		if (newHealth <= 0)
		{
			Destroy(this.gameObject);
		}
	}
}
