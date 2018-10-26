﻿using Harmony;
using UnityEngine;

namespace DefaultNamespace.Collectable
{
    public class Collectable:MonoBehaviour
    {
        [SerializeField] private int scoreValue;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == Values.Tags.Player)
            {
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().AddCollectable(scoreValue);
                Destroy(this.gameObject);
            }
        }
    }
}