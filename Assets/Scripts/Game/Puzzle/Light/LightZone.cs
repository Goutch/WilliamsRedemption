using System.Collections;
using System.Collections.Generic;
using Game.Puzzle.Light;
using Harmony;
using UnityEngine;

public class LightZone : MonoBehaviour
{
    [SerializeField] private List<MeshLight> Lights;
    private BoxCollider2D box;

    private void Awake()
    {
        box = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        DisableLights();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.Root().CompareTag("Player"))
        {
            EnableLights();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.Root().CompareTag("Player"))
        {
            DisableLights();
        }
    }

    public void DisableLights()
    {
        foreach (var light in Lights)
        {
            if (light.UpdateEveryFrame)
            {
                light.transform.root.gameObject.active = false;
            }
        }
    }

    public void EnableLights()
    {
        foreach (var light in Lights)
        {
            if (light.UpdateEveryFrame)
            {
                light.transform.root.gameObject.active = true;
            }
        }
    }
}