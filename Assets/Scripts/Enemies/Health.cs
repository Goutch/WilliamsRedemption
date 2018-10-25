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

	public int HealthPoints
	{
		get { return healthPoints; }
		private set
        {
            healthPoints = value;
            OnHealthChange?.Invoke(gameObject);

            if (IsDead())
            {
                OnDeath?.Invoke(gameObject);
	            if (isKilledByPlayer && IsAnEnemy())
	            {
		            AddEnemyScoreToGameScore();
	            }
                Destroy(this.transform.root.gameObject);
            }
        }
	}

	void Awake () {
        healthPoints = MaxHealth;
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
