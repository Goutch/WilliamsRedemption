using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerSpecificSounds : MonoBehaviour
{
    private AudioSource source;
    private AudioClip clip;
    private GameObject linkedObject;
    private bool doesSoundStopOnObjectDestroy;
    private float soundValue = 1F; //BEN_CORRECTION : Jamais modifié.

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void Init(AudioClip clip, bool doesSoundStopOnObjectDestroy, GameObject linkedObject)
    {
        this.clip = clip;
        this.doesSoundStopOnObjectDestroy = doesSoundStopOnObjectDestroy;
        this.linkedObject = linkedObject;
    }

    private void Update()
    {
        if (linkedObject != null)
        {
            transform.position = linkedObject.transform.position;
        }
        else if (linkedObject == null && doesSoundStopOnObjectDestroy)
        {
            Destroy(gameObject);
        }

        if (!source.isPlaying)
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound()
    {
        source.PlayOneShot(clip, soundValue);
    }
}