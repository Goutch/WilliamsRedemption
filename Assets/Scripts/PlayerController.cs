using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] private GameObject william;
    [SerializeField] private GameObject reaper;

    private void Start()
    {
        OnLightEnter();
    }

    public void OnLightEnter()
    {
        william.SetActive(true);
        reaper.SetActive(false);
    }

    public void OnLightLeft()
    {
        william.SetActive(false);
        reaper.SetActive(true);
    }
}
