using Game.Entity;
using Game.Entity.Player;
using UnityEngine;

//BEN_CORRECTION : Toute l'équipe a utilisé des "namespace" sauf toi. Non respect du standard d'équipe.
//BEN_CORRECTION : Classe à reconcevoir. Il y a des façons beaucoup plus simple de faire ce qu'elle fait.
//BEN_CORRECTION : Concerne uniquement la musique, mais le nom ne le reflète pas. Contient aussi le terme
//                 "Manager", ce qui n'est pas une bonne pratique (considéré comme un "bad smell").
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

    //BEN_CORRECTION : Nommage. Stop music non ?
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