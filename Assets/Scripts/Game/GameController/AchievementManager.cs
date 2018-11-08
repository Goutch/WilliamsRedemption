using System.CodeDom;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Game.Controller
{
    public class AchievementManager:MonoBehaviour
    {
        private GameController gameController;
        private int numberCollectableRemaining=0;
        Dictionary<int, List<Achievement>> Achievements;
        private void Start()
        {
            numberCollectableRemaining = GameObject.FindGameObjectsWithTag(Values.Tags.Collectable).Length;
            gameController = GameObject.FindGameObjectWithTag(Values.Tags.GameController).GetComponent<GameController>();
        }
        
    }
}