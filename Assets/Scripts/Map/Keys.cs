using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keys: MonoBehaviour
{
    [Tooltip("Door tied to this key.")]
    [SerializeField] private Doors door;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            door.Unlock();
            gameObject.SetActive(false);
        }
    }
}