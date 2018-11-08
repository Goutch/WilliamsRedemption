using System.Collections.Generic;
using Game.Entity.Enemies;
using Game.Entity.Enemies.Boss;
using Game.Values;
using JetBrains.Annotations;
using UnityEngine;

namespace Game.Controller
{
    public delegate void AchievementEventHandler();

    public class AchievementEventChannel:MonoBehaviour
    {
        private AchievementManager achievementManager;
        public event AchievementEventHandler OnPlayerFindCollectable;
        private List<Achievement> acomplishedAchievements;
        [SerializeField]private List<Achievement> achievement;
        private void Start()
        {
            achievementManager = GetComponent<AchievementManager>();}

        public void OnEnemyDeath(Enemy enemy)
        {
            switch (enemy.name)
            {
                case Values.GameObject.Jacob:
                    
                    break;
                case Values.GameObject.Edgar:
                    
                    break;
                case Values.GameObject.Jean:
                    
                    break;
                case Values.GameObject.Anna:
                    
                    break;
                case Values.GameObject.ZekGor:
                    
                    break;
                case Values.GameObject.Death:
                    
                    break;
                default:
                    break;
            }
        }

        public void OnPlayerTakeDamage()
        {
            
        }

        public void PublishPlayerFoundCollectable()
        {
            
        }
    }
}