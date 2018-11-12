using System.Collections.Generic;
using Game.Entity.Enemies;
using Game.Entity.Enemies.Boss;
using Game.Values;
using JetBrains.Annotations;
using UnityEngine;

namespace Game.Controller
{
    public delegate void AchievementEventHandler();

    public class AchievementEventChannel : MonoBehaviour
    {
        private AchievementManager achievementManager;
        public event AchievementEventHandler OnPlayerFindCollectable;
        public event AchievementEventHandler OnPlayerKillJacob;
        public event AchievementEventHandler OnPlayerKillEdgar;
        public event AchievementEventHandler OnPlayerKillJean;
        public event AchievementEventHandler OnPlayerKillAnna;
        public event AchievementEventHandler OnPlayerKillZekGor;
        public event AchievementEventHandler OnPlayerKillTheDeath;

        private void Start()
        {
            achievementManager = GetComponent<AchievementManager>();
        }

        public void OnEnemyDeath(Enemy enemy)
        {
            switch (enemy.name)
            {
                case Values.GameObject.Jacob:
                    OnPlayerKillJacob?.Invoke();
                    break;
                case Values.GameObject.Edgar:
                    OnPlayerKillEdgar?.Invoke();
                    break;
                case Values.GameObject.Jean:
                    OnPlayerKillJean?.Invoke();
                    break;
                case Values.GameObject.Anna:
                    OnPlayerKillAnna?.Invoke();
                    break;
                case Values.GameObject.ZekGor:
                    OnPlayerKillZekGor?.Invoke();
                    break;
                case Values.GameObject.Death:
                    OnPlayerKillTheDeath?.Invoke();
                    break;
                default:
                    break;
            }
        }

        public void PublishPlayerFoundCollectable()
        {
        }
    }
}