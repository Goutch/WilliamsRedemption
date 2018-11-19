using System.ComponentModel;
using System.Runtime.InteropServices;
using Game.Entity;
using Game.Entity.Player;
using UnityEngine;

namespace Game.Controller
{
    public class Checkpoint:MonoBehaviour
    {
        private GameController gamecontroller;
        private PlayerController player;
        private bool trigerred;
        private int healthPointAtTimeOfTrigger = 0;
        private int scoreAtTimeOfTrigger;
        private float timeAtTimeOfTrigger;

        public int HealthPointAtTimeOfTrigger => healthPointAtTimeOfTrigger;

        public int ScoreAtTimeOfTrigger => scoreAtTimeOfTrigger;

        public float TimeAtTimeOfTrigger => timeAtTimeOfTrigger;


        private void Awake()
        {
            gamecontroller = GameObject.FindGameObjectWithTag(Values.GameObject.GameController)
                .GetComponent<GameController>();
            player= GameObject.FindGameObjectWithTag(Values.Tags.Player)
                .GetComponent<PlayerController>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!trigerred&&gamecontroller.ExpertMode == false)
            {
                trigerred = true;
                healthPointAtTimeOfTrigger = player.GetComponent<Health>().HealthPoints;
                timeAtTimeOfTrigger = Time.time;
                scoreAtTimeOfTrigger = gamecontroller.Score;
                gamecontroller.OnCheckPointTrigerred(this);
            }
        }
    }
}