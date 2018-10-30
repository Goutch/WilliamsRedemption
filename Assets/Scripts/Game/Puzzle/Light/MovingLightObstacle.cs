using System;
using UnityEngine;

namespace Game.Puzzle.Light
{
    public class MovingLightObstacle : MonoBehaviour
    {
        private void Awake()
        {
            if (!transform.root.GetComponent<Collider2D>())
            {
                throw new Exception("A lightSensor need a collider2D");
            }

            if (!transform.root.GetComponent<Rigidbody2D>())
            {
                throw new Exception("A lightSensor need a RigidBody2D");
            }
        }
    }
}