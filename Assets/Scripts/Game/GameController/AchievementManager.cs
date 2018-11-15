using System.Collections.Generic;
using Game.Controller.Events;
using Game.Entity.Enemies;
using Game.Entity.Enemies.Boss;
using Game.Entity.Enemies.Boss.Edgar;
using Game.Entity.Enemies.Boss.Jacob;
using Game.Entity.Enemies.Boss.Jean;
using Game.Entity.Enemies.Boss.Zekgor;
using Game.UI;
using UnityEngine;

namespace Game.Controller
{
    public class AchievementManager:MonoBehaviour
    {
        [SerializeField] private AchievementUI achievementUi;
        [Tooltip("PhantomCanHang number of ghost to unlock achievement")][SerializeField] private int PhantomCanHang=10;
        private GameController gameController;
        private string achievementPath = "Achievements";
        private List<Achievement> acomplishedAchievements;
        private Dictionary<string,Achievement> achievements;
        public List<Achievement> AcomplishedAchievements => acomplishedAchievements;

        private int zombieKillCount = 0;
        private int ghostKillCount=0;
        private int batKillCount = 0;
        private int sorcererKillCount = 0;

        private void Start()
        {
            acomplishedAchievements=new List<Achievement>();
            gameController=GetComponent<GameController>();
            gameController.OnGameEnd += OnGameEnd;
            achievements=new Dictionary<string, Achievement>();
            foreach (var achievement in Resources.LoadAll<Achievement>(achievementPath))
            {
                achievements.Add(achievement.name,achievement);
            } 
            gameController.GetComponent<EnemyDeathEventChannel>().OnEventPublished += OnEnemyDie;

        }

        private void OnGameEnd()
        {
            
        }

        private void OnEnemyDie(OnEnemyDeath enemyDeath)
        {
            if (enemyDeath.Enemy is BossController)
            {
                if (enemyDeath.Enemy is JacobController)
                {
                    acomplishedAchievements.Add(achievements[Values.Achievements.ZombieSlayer]);
                }
                else if (enemyDeath.Enemy is EdgarController)
                {
                    acomplishedAchievements.Add(achievements[Values.Achievements.KnightSlayer]);
                }
                else if (enemyDeath.Enemy is JeanController)
                {
                    acomplishedAchievements.Add(achievements[Values.Achievements.PriestSlayer]);
                }
               // else if(enemyDeath.Enemy is AnnaController)
               // {
               //     acomplishedAchievements.Add(achievements[Values.Achievements.KnightSlayer]);
               // }
                else if(enemyDeath.Enemy is ZekgorController)
                {
                    acomplishedAchievements.Add(achievements[Values.Achievements.DemonSlayer]);
                }
              // else if(enemyDeath.Enemy is DeathController)
              // {
              //     acomplishedAchievements.Add(achievements[Values.Achievements.KnightSlayer]);
              // }
            }
            else
            {
                if (enemyDeath.Enemy is Zombie)
                {
                    zombieKillCount++;
                }

                else if (enemyDeath.Enemy is Ghost)
                {
                    ghostKillCount++;
                    if (ghostKillCount == PhantomCanHang)
                    {
                        acomplishedAchievements.Add(achievements[Values.Achievements.PhantomsCanHang]);
                    }
                }
                else if(enemyDeath.Enemy is Bat)
                {
                    batKillCount++;
                }
                else if (enemyDeath.Enemy is Sorcerer)
                {

                    sorcererKillCount++;
                }
            }
        }
        
        
    }
}