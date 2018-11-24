using Game.Entity;
using Game.Entity.Player;
using UnityEngine;

public class AudioManagerBackgroundSound : MonoBehaviour
{
    private AudioSource source;
    private AudioClip clip;
    private bool shouldMusicBePlaying;
    private bool isAMusicTrigger;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        shouldMusicBePlaying = false;
    }

    private void Start()
    {
        PlayerController.instance.GetComponent<Health>().OnDeath += OnPlayerDie;
    }

    public void Init(AudioClip clip)
    {
        this.clip = clip;
        shouldMusicBePlaying = true;
    }

    private void Update()
    {
        if (!source.isPlaying && shouldMusicBePlaying)
        {
            PlaySound();
        }
        else if (!source.isPlaying && !shouldMusicBePlaying)
        {
            StopSound();
        }

        if (isAMusicTrigger)
        {
            shouldMusicBePlaying = false;
        }
    }

    private void OnPlayerDie(GameObject gameObject, GameObject gameObject2)
    {
        StopSound();
    }

    public void PlaySound()
    {
        source.PlayOneShot(clip, 1F);
    }

    public void StopSound()
    {
        source.Stop();
        shouldMusicBePlaying = false;
    }

    public void UpdateMusicTriggers()
    {
        isAMusicTrigger = true;
    }
}