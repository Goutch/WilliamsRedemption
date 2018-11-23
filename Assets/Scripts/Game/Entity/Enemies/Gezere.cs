using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Game.Entity;
using Game.Puzzle.Light;
using TMPro;
using UnityEngine;

public class Gezere : MonoBehaviour
{
    [SerializeField] private float OpenTime;
    [SerializeField] private float closedTime;
    [SerializeField] private Collider2D attackCollider;
    private new CircleLight light;


    private ParticleSystem particules;

    // Use this for initialization
    private void Start()
    {
        light = GetComponentInChildren<CircleLight>();
        particules = GetComponentInChildren<ParticleSystem>();
        StartCoroutine(openAndCloseRoutine());
    }

    private void Open()
    {
        light.Open();
        particules.Play();
        attackCollider.enabled = true;
    }

    private void Close()
    {
        light.Close();
        particules.Stop();
        attackCollider.enabled = false;
    }

    private IEnumerator openAndCloseRoutine()
    {
        Open();
        yield return new WaitForSeconds(OpenTime);
        Close();
        yield return new WaitForSeconds(closedTime);
        StartCoroutine(openAndCloseRoutine());
    }
}