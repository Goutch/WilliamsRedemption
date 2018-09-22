using System;
using UnityEngine;

namespace Light
{
    public class MovingLightObstacle:MonoBehaviour
    {
        private void Awake()
        {
            if (!GetComponent<Collider2D>())
            {
                throw new Exception("A lightSensor need a collider2D");
            }

            if (!GetComponent<Rigidbody2D>())
            {
                throw new Exception("A lightSensor need a RigidBody2D");
            }
        }
    }
}