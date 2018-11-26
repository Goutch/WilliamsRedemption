using System.Collections.Generic;
using Game.Controller.Events;
using Game.Entity.Enemies;
using Game.Entity.Enemies.Boss;
using Game.Entity.Enemies.Boss.Edgar;
using Game.Entity.Enemies.Boss.Jacob;
using Game.Entity.Enemies.Boss.Jean;
using Game.Entity.Enemies.Boss.Zekgor;
using Game.UI;
using Game.Values.AnimationParameters;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using Anna = Game.Entity.Enemies.Boss.Anna.Anna;
using Death = Game.Entity.Enemies.Boss.Death.Death;

namespace Game.Controller
{
    public class AchievementManager : MonoBehaviour
    {
        private AchievementUI achievementUi;


        [Tooltip("PhantomCanHang number of ghost to kill to unlock Phantoms can hang")] [SerializeField]
        private int PhantomCanHang = 10;

        [Tooltip("Number of jump to unluck BunnyHop")] [SerializeField]
        private int BunnyHop = 500;

        [Tooltip("Number of second to unlock super sonic")] [SerializeField]
        private int supersonic = 600;

        [Tooltip("Number of shoot to unlock trigger happy")] [SerializeField]
        private int triggerHappy = 1;

        private GameController gameController;
        private string achievementPath = "Achievements";
        private List<Achievement> acomplishedAchievements;
        private Dictionary<string, Achievement> achievements;

        private int zombieKillCount = 0;
        private int ghostKillCount = 0;
        private int batKillCount = 0;
        private int sorcererKillCount = 0;
        private int collectableLevel1Count = 0;
        private int collectableLevel2Count = 0;
        private int collectableLevel3Count = 0;
        private int Level1PlayerDamageCount = 0;
        private int Level2PlayerDamageCount = 0;
        private int Level3PlayerDamageCount = 0;

        private int playerJumpCount = 0;
        private int playerShootCount = 0;

        private void Start()
        {
            achievements = new Dictionary<string, Achievement>();
            acomplishedAchievements = new List<Achievement>();
            gameController = GetComponent<GameController>();

            achievementUi = GetComponent<AchievementUI>();

            gameController.OnGameEnd += OnGameEnd;
            gameController.OnLevelChange += OnLevelChange;

            foreach (var achievement in Resources.LoadAll<Achievement>(achievementPath))
            {
                achievements.Add(achievement.name, achievement);
            }

            GetComponent<EnemyDeathEventChannel>().OnEventPublished += OnEnemyDie;
            GetComponent<CollectablesEventChannel>().OnEventPublished += CollectableFound;
            GetComponent<PlayerJumpEventChannel>().OnEventPublished += PlayerJump;
            GetComponent<PlayerHealthEventChannel>().OnEventPublished += PlayerTakeDamage;
            GetComponent<PlayerShootEventChannel>().OnEventPublished += PlayerShoot;
        }

        private void OnDisable()
        {
            gameController.OnGameEnd -= OnGameEnd;
            GetComponent<EnemyDeathEventChannel>().OnEventPublished -= OnEnemyDie;
            GetComponent<CollectablesEventChannel>().OnEventPublished -= CollectableFound;
            GetComponent<PlayerJumpEventChannel>().OnEventPublished -= PlayerJump;
            GetComponent<PlayerHealthEventChannel>().OnEventPublished -= PlayerTakeDamage;
            GetComponent<PlayerShootEventChannel>().OnEventPublished -= PlayerShoot;
            gameController.OnLevelChange -= OnLevelChange;
        }

        private void OnLevelChange()
        {
            if (gameController.CurrentLevel == null)
            {
                return;
            }

            if (gameController.CurrentLevel.Scene == Values.Scenes.Level1)
            {
                if (Level1PlayerDamageCount == 0)
                {
                    acomplishedAchievements.Add(achievements[Values.Achievements.Legendary1]);
                }
            }

            else if (gameController.CurrentLevel.Scene == Values.Scenes.Level2)
            {
                if (Level2PlayerDamageCount == 0)
                {
                    acomplishedAchievements.Add(achievements[Values.Achievements.Legendary2]);
                }
            }

            else if (gameController.CurrentLevel.Scene == Values.Scenes.Level3)
            {
                if (Level3PlayerDamageCount == 0)
                {
                    acomplishedAchievements.Add(achievements[Values.Achievements.Legendary3]);
                }
            }
        }

        private void OnGameEnd()
        {
            if (gameController.IsGameWinned && gameController.TotalTime <= supersonic)
            {
                acomplishedAchievements.Add(achievements[Values.Achievements.SuperSonic]);
            }

            if (playerShootCount >= triggerHappy)
            {
                acomplishedAchievements.Add(achievements[Values.Achievements.TriggerHappy]);
            }

            if (acomplishedAchievements.Count == achievements.Count - 1)
            {
                acomplishedAchievements.Add(achievements[Values.Achievements.Perfection]);
            }


            int bonusScoreValue = 0;

            string achievementListText = "";
            foreach (var achievement in acomplishedAchievements)
            {
                achievementListText += achievement.name;
                achievementListText += ":";
                achievementListText += achievement.ScoreValue;
                achievementListText += "\n";
                bonusScoreValue += achievement.ScoreValue;
            }

            achievementUi.DisplayAchievements(achievementListText);
            gameController.AddBonusScore(bonusScoreValue);
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
                else if(enemyDeath.Enemy is Anna)
                {
                    acomplishedAchievements.Add(achievements[Values.Achievements.TangledChains]);
                }
                else if (enemyDeath.Enemy is ZekgorController)
                {
                    acomplishedAchievements.Add(achievements[Values.Achievements.DemonSlayer]);
                }

                else if(enemyDeath.Enemy is Death)
                {
                    acomplishedAchievements.Add(achievements[Values.Achievements.DeathByDeath]);
                }
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
                else if (enemyDeath.Enemy is Bat)
                {
                    batKillCount++;
                }
                else if (enemyDeath.Enemy is Sorcerer)
                {
                    sorcererKillCount++;
                }
            }
        }

        private void CollectableFound(OnCollectableFound collectableEvent)
        {
            if (collectableEvent.Level.Scene == Values.GameObject.Level1)
            {
                collectableLevel1Count++;
                if (collectableLevel1Count == collectableEvent.Level.NumberCollectables)
                {
                    acomplishedAchievements.Add(achievements[Values.Achievements.Archeologue1]);
                }
            }

            if (collectableEvent.Level.Scene == Values.GameObject.Level2)
            {
                collectableLevel2Count++;
                if (collectableLevel2Count == collectableEvent.Level.NumberCollectables)
                {
                    acomplishedAchievements.Add(achievements[Values.Achievements.Archeologue2]);
                }
            }

            if (collectableEvent.Level.Scene == Values.GameObject.Level3)
            {
                collectableLevel3Count++;
                if (collectableLevel3Count == collectableEvent.Level.NumberCollectables)
                {
                    acomplishedAchievements.Add(achievements[Values.Achievements.Archeologue3]);
                }
            }
        }

        private void PlayerJump(OnPlayerJump playerJumpEvent)
        {
            playerJumpCount++;
            if (playerJumpCount == BunnyHop)
            {
                acomplishedAchievements.Add(achievements[Values.Achievements.BunnyHop]);
            }
        }

        private void PlayerTakeDamage(OnPlayerTakeDamage onPlayerTakeDamageEvent)
        {
            if (gameController.CurrentLevel.Scene == Values.GameObject.Level1)
            {
                Level1PlayerDamageCount++;
            }

            if (gameController.CurrentLevel.Scene == Values.GameObject.Level2)
            {
                Level2PlayerDamageCount++;
            }

            if (gameController.CurrentLevel.Scene == Values.GameObject.Level2)
            {
                Level3PlayerDamageCount++;
            }
        }

        private void PlayerShoot(OnPlayerShoot shoot)
        {
            playerShootCount++;
        }
    }
}