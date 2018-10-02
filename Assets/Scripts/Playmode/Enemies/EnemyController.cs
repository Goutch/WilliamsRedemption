using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	[SerializeField] private float distanceToDetectThePlayer;
	
	private Health health;
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
		if (hitSensor != null)
		{
			hitSensor.OnHit += OnHit;
		}	
	}

	private void OnHit()
	{
		health.Hit();
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
	protected bool CheckIfEnemyIsSeen()
	{
		if(GameObject.FindGameObjectWithTag("Player").transform.position.x-gameObject.transform.position.x
		   <=distanceToDetectThePlayer)
		{
			Debug.Log("Enemy spotted!");
			return true;
		}
		return false;
	}
}
