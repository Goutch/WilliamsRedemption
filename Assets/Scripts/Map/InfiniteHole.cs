using System.Collections;
using System.Collections.Generic;
using Harmony;
using UnityEngine;

public class InfiniteHole : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.Root().CompareTag("Player"))
        {
            other.Root().transform.position = respawnPoint.position;
            other.transform.root.GetComponent<PlayerController>().DamagePlayer();
        }
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Health>().Kill();
        }
    }
}