using Game.Entity;
using Game.Entity.Player;
using UnityEngine;
using Game.Controller;

public class AudioManagerBackgroundSound : MonoBehaviour
{
    [SerializeField] private float soundValue;
    
    private AudioSource source;
    private AudioClip clip;
    private bool shouldMusicBePlaying;

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
    }

    private void OnPlayerDie(GameObject gameObject, GameObject gameObject2)
    {
        StopSound();
    }

    public void PlaySound()
    {
        source.PlayOneShot(clip, soundValue);
    }

    public void StopSound()
    {
        source.Stop();
        shouldMusicBePlaying = false;
    }

    public void TimerSoundStop()
    {
        StopSound();
        clip = GameObject.FindGameObjectWithTag(Game.Values.Tags.GameController)
            .GetComponent<GameController>().CurrentLevel.LevelMusic;
        Init(clip);
    }
}