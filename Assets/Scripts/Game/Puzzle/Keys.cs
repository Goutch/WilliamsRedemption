﻿using UnityEngine;

namespace Game.Puzzle
{
    //BEN_REVIWE : Key pas de "s" ? Non ?
    public class Keys : MonoBehaviour
    {
        [Tooltip("Door tied to this key.")]
        [SerializeField] private Doors door;


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(Values.Tags.Player))
            {
                door.Unlock();
                gameObject.SetActive(false);
            }
        }
    }
}

