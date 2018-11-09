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
    private CircleLight light;
    private Collider2D attackColider;

    private ParticleSystem particules;

    // Use this for initialization
    private void Start()
    {
        light = GetComponentInChildren<CircleLight>();
        attackColider = GetComponentInChildren<HitStimulus>().GetComponent<Collider2D>();
        particules = GetComponentInChildren<ParticleSystem>();
        StartCoroutine(openAndCloseRoutine());
    }

    private void Open()
    {
        light.Open();
        particules.Play();
        attackColider.enabled = true;
    }

    private void Close()
    {
        light.Close();
        particules.Stop();
        attackColider.enabled = false;
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