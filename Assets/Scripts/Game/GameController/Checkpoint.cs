﻿using System.ComponentModel;
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
            private float timeAtTimeOfTrigger;
            private Vector3 positionAtTimeOfTrigger;

            public int HealthPointAtTimeOfTrigger => healthPointAtTimeOfTrigger;

            public int ScoreAtTimeOfTrigger => scoreAtTimeOfTrigger;

            public float TimeAtTimeOfTrigger => timeAtTimeOfTrigger;

            public Vector3 PositionAtTimeOfTrigger => positionAtTimeOfTrigger;

            public CheckPointData(int health,int score,float time,Vector3 position)
            {
                healthPointAtTimeOfTrigger = health;
                scoreAtTimeOfTrigger = score;
                timeAtTimeOfTrigger = time;
                positionAtTimeOfTrigger = position;
            }
        }

        private bool trigerred;
        private GameController gamecontroller;
        private PlayerController player;
        private CheckPointData data;
        public CheckPointData Data=>data;

        private void Awake()
        {
            gamecontroller = GameObject.FindGameObjectWithTag(Values.GameObject.GameController)
                .GetComponent<GameController>();
            player = GameObject.FindGameObjectWithTag(Values.Tags.Player)
                .GetComponent<PlayerController>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!trigerred && gamecontroller.ExpertMode == false && other.Root().tag == Values.Tags.Player)
            {
                trigerred = true;
                data=new CheckPointData(player.GetComponent<Health>().HealthPoints,gamecontroller.Score,Time.time,transform.position);
                gamecontroller.OnCheckPointTrigerred(data);
            }
        }
    }
}