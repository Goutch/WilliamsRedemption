using AnimatorExtension;
using Game.Controller;
using Game.Controller.Events;
using Game.Entity.Enemies;
using UnityEngine;

namespace Game.Entity
{
    public delegate void HealthEventHandler(GameObject gameObject);

    public class Health : MonoBehaviour
    {
        [SerializeField] private int maxHealth;
        public int MaxHealth => maxHealth;

        public event HealthEventHandler OnDeath;
        public event HealthEventHandler OnHealthChange;
        private GameController gameController;

        private Animator animator;

        private bool isKilledByPlayer = true;

        private int healthPoints;
        public int HealthPoints
        {
            get { return healthPoints; }
            private set
            {
                healthPoints = value;
                OnHealthChange?.Invoke(gameObject);

                if (animator != null && animator.ContainsParam(Values.AnimationParameters.Enemy.Hurt))
                    animator.SetTrigger(Values.AnimationParameters.Enemy.Hurt);

                if (IsDead())
                {
                    if (IsAnEnemy())
                    {
                        gameController.GetComponent<EnemyDeathEventChannel>()
                            .Publish(new OnEnemyDeath(GetComponent<Enemy>()));
                        if (isKilledByPlayer)
                        {
                            AddEnemyScoreToGameScore();
                        }
                        
                    }

                    OnDeath?.Invoke(transform.root.gameObject);
                }
            }
        }

        void Awake()
        {
            healthPoints = MaxHealth;
            animator = GetComponent<Animator>();
            gameController = GameObject.FindGameObjectWithTag(Values.Tags.GameController)
                .GetComponent<GameController>();
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
            GameObject.FindGameObjectWithTag(Values.Tags.GameController).GetComponent<GameController>().AddScore(score);
        }

        private bool IsAnEnemy()
        {
            return GetComponent<Enemy>() != null;
        }
    }
}