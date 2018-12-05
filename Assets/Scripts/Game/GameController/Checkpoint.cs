using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Game.Entity;
using Game.Entity.Player;
using Game.Puzzle;
using Harmony;
using UnityEngine;

namespace Game.Controller
{
    public class Checkpoint : MonoBehaviour
    {
        public struct CheckPointData
        {
            private int healthPointAtTimeOfTrigger;
            private int scoreAtTimeOfTrigger;
            private Vector3 positionAtTimeOfTrigger;
            private List<Doors> doorsToOpenOnRespawn;
            public float time;

            public int HealthPointAtTimeOfTrigger => healthPointAtTimeOfTrigger;

            public int ScoreAtTimeOfTrigger => scoreAtTimeOfTrigger;

            public Vector3 PositionAtTimeOfTrigger => positionAtTimeOfTrigger;

            public List<Doors> DoorsToOpenOnRespawn => doorsToOpenOnRespawn;

            public CheckPointData(int health, int score, float time, Vector3 position , List<Doors> allDoors)
            {
                healthPointAtTimeOfTrigger = health;
                scoreAtTimeOfTrigger = score;
                positionAtTimeOfTrigger = position;
                doorsToOpenOnRespawn = allDoors;
                this.time = time;
            }
        }

        [SerializeField] private List<Doors> DoorsToOpen;
        private GameController gamecontroller;
        private PlayerController player;
        private CheckPointData data;
        public CheckPointData Data => data;

        private bool canBeTriggered;

        private void Awake()
        {
            gamecontroller = GameObject.FindGameObjectWithTag(Values.GameObject.GameController)
                .GetComponent<GameController>();
            player = GameObject.FindGameObjectWithTag(Values.Tags.Player)
                .GetComponent<PlayerController>();
            canBeTriggered = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (CheckIfCheckPointCanBeTriggered() && !gamecontroller.ExpertMode &&
                other.Root().CompareTag(Values.Tags.Player))
            {
                data = new CheckPointData(player.GetComponent<Health>().HealthPoints, gamecontroller.Score,
                    gamecontroller.LevelRemainingTime,
                    transform.position,DoorsToOpen);
                gamecontroller.OnCheckPointTrigerred(data);
            }
        }

        private bool CheckIfCheckPointCanBeTriggered()
        {
            if (DoorsToOpen.Count ==0)
            {
                return true;
            }
            else
            {
                foreach (var door in DoorsToOpen)
                {
                    if (door.IsLocked())
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}