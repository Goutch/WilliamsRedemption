using System.CodeDom;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Game.UI;
using UnityEngine;

namespace Game.Controller
{
    public class AchievementManager:MonoBehaviour
    {
        [SerializeField] private AchievementUI achievementUi;
        private GameController gameController;
        private string achievementPath = "Achievements";
        private List<Achievement> acomplishedAchievements;
        private Dictionary<string,Achievement> achievements;
        private AchievementEventChannel achievementEventChannel;

        
        private void Start()
        {
            gameController = GameObject.FindGameObjectWithTag(Values.Tags.GameController).GetComponent<GameController>();
            gameController.OnGameEnd += OnGameEnd;

            achievementEventChannel.OnPlayerFindCollectable += OnPlayerFindCollectable;
            foreach (var achievement in Resources.LoadAll<Achievement>(achievementPath))
            {
                achievements.Add(achievement.name,achievement);
            } 
            achievements=new Dictionary<string, Achievement>();
        }

        private void OnGameEnd()
        {
            
        }

        private void OnPlayerFindCollectable()
        {
            
        }

        private void OnPlayerKillEdgar()
        {
            
        }

        private void OnPlayerKillJacob()
        {
            
        }

        private void OnPlayerKillJean()
        {
            
        }

        private void OnPlayerKillAnna()
        {
            
        }

        private void OnPlayerKillZekGor()
        {
            
        }

        private void OnPlayerKillTheDeath()
        {
            
        }
        
        
    }
}