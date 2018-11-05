using System;
using UnityEngine;

namespace Game.Puzzle.Light
{
    //BEN_CORRECTION : Il n'y a, pour ainsi dire, rien la dedans. Pourtant, c'est utilisé à plusieurs endroits dans "LightStimulus".
    //
    //                 Est-ce que c'est toujours d'actualité ce truc là ? Si ce n'est qu'un marqueur, c'est possible de l'indiquer
    //                 à quelque part ?
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