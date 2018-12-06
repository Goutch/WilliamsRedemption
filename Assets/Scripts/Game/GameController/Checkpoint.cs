using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Game.Entity;
using Game.Entity.Enemies.Boss;
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
            private string checkPointName;
            public float time;

            public int HealthPointAtTimeOfTrigger => healthPointAtTimeOfTrigger;

            public int ScoreAtTimeOfTrigger => scoreAtTimeOfTrigger;

            public Vector3 PositionAtTimeOfTrigger => positionAtTimeOfTrigger;

            public String CheckPointName => checkPointName;

            public CheckPointData(int health, int score, float time, Vector3 position, String name)
            {
                healthPointAtTimeOfTrigger = health;
                scoreAtTimeOfTrigger = score;
                positionAtTimeOfTrigger = position;
                checkPointName = name;
                this.time = time;
            }
        }

        [Header("BossFight")]
        [Tooltip("Check this box if a Boss Fight needs to be disabled when the player respawns.")]
        [SerializeField]
        private bool DisableBossFight;

        [Tooltip("This BossFight will be disabled when the player respawns if DisableBossFight is true.")]
        [SerializeField]
        private BossFight FightToDisableOnRespawn;

        [Header("Checkpoints")]
        [Tooltip("Check this box to disable all previous checkpoints when this one is triggered.")]
        [SerializeField]
        private bool DisablePreviousCheckPoints;

        [Tooltip(
            "Contains every checkPoint that will be disabled when this one is triggered. (If DisablePreviousCheckPoints is true).")]
        [SerializeField]
        private List<Checkpoint> CheckPointsToDisableOnRespawn;

        [Header("Doors")] [SerializeField] private List<Doors> DoorsToOpen;

        [Tooltip("Check this box to disable all previous keys when this checkpoint is triggered.")]
        [Header("Keys")]
        [SerializeField]
        private bool DisablePreviousKeys;

        [Tooltip(
            "Contains every key that will be disabled when this checkpoint is triggered (If DisablePreviousKeys is true.)")]
        [SerializeField]
        private List<Keys> KeysToDisable;

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
            if (CheckIfCheckPointCanBeTriggered(other))
            {
                if (!gamecontroller.ExpertMode)
                {
                    data = new CheckPointData(player.GetComponent<Health>().HealthPoints, gamecontroller.Score,
                        gamecontroller.LevelRemainingTime,
                        transform.position, gameObject.name);
                    gamecontroller.OnCheckPointTrigerred(data);
                    gamecontroller.LastCheckpoint = this;
                }

                if (DisableBossFight && FightToDisableOnRespawn.isActiveAndEnabled)
                {
                    DisableBossFightOnTrigger();
                    DisableBossFight = false;
                }

                if (DisablePreviousCheckPoints)
                {
                    DisablePreviousCheckPointsOnTrigger();
                    DisablePreviousCheckPoints = false;
                }

                if (DisablePreviousKeys)
                {
                    DisablePreviousKeysOnTriggers();
                    DisablePreviousKeys = false;
                }
            }
        }

        private bool CheckIfCheckPointCanBeTriggered(Collider2D other)
        {
            if (DoorsToOpen.Count == 0)
            {
                return true;
            }

            if (!other.Root().CompareTag(Values.Tags.Player))
            {
                return false;
            }

            foreach (var door in DoorsToOpen)
            {
                if (door.IsLocked() && !door.IsOpened())
                {
                    return false;
                }
            }

            return true;
        }

        public void UnlockDoors()
        {
            foreach (var door in DoorsToOpen)
            {
                door.Unlock();
            }
        }

        private void DisableBossFightOnTrigger()
        {
            FightToDisableOnRespawn.gameObject.SetActive(false);
        }

        private void DisablePreviousCheckPointsOnTrigger()
        {
            foreach (var checkPoint in CheckPointsToDisableOnRespawn)
            {
                checkPoint.gameObject.SetActive(false);
            }
        }

        private void DisablePreviousKeysOnTriggers()
        {
            foreach (var key in KeysToDisable)
            {
                key.gameObject.SetActive(false);
            }
        }
    }
}