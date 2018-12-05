using System.ComponentModel;
using System.Runtime.InteropServices;
using Game.Entity;
using Game.Entity.Player;
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
            public float time;

            public int HealthPointAtTimeOfTrigger => healthPointAtTimeOfTrigger;

            public int ScoreAtTimeOfTrigger => scoreAtTimeOfTrigger;

            public Vector3 PositionAtTimeOfTrigger => positionAtTimeOfTrigger;

            public CheckPointData(int health, int score, float time, Vector3 position)
            {
                healthPointAtTimeOfTrigger = health;
                scoreAtTimeOfTrigger = score;
                positionAtTimeOfTrigger = position;
                this.time = time;
            }
        }

        private bool trigerred;
        private GameController gamecontroller;
        private PlayerController player;
        private CheckPointData data;
        public CheckPointData Data => data;

        private void Awake()
        {
            gamecontroller = GameObject.FindGameObjectWithTag(Values.GameObject.GameController)
                .GetComponent<GameController>();
            player = GameObject.FindGameObjectWithTag(Values.Tags.Player)
                .GetComponent<PlayerController>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!trigerred && gamecontroller.ExpertMode == false && other.Root().CompareTag(Values.Tags.Player))
            {
                trigerred = true;
                data = new CheckPointData(player.GetComponent<Health>().HealthPoints, gamecontroller.Score, gamecontroller.LevelRemainingTime,
                    transform.position);
                gamecontroller.OnCheckPointTrigerred(data);
            }
        }
    }
}