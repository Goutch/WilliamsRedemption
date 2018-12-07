using System.Collections;
using Game.Controller;
using Harmony;
using UnityEngine;

namespace Game.Audio
{
    public class MusicTriggers : MonoBehaviour
    {
        
        [SerializeField] private int TrackIndex;
        [SerializeField] private float AudioFadeTime;

        private AudioManagerBackgroundSound audioManager;

        private bool isTriggered;

        private AudioClip levelMusic;

        private Level currentLevel;

        private float startVolume;

        private AudioSource audioSource;

        private bool musicIsPlaying;

        private void Awake()
        {
            audioManager = GameObject.FindGameObjectWithTag(Values.Tags.MainCamera)
                .GetComponent<AudioManagerBackgroundSound>();
            currentLevel = GameObject.FindGameObjectWithTag(Values.Tags.GameController)
                .GetComponent<GameController>().CurrentLevel;
            musicIsPlaying = false;        
            isTriggered = false;
        }

        private void Start()
        {
            audioSource = GameObject.FindGameObjectWithTag(Values.Tags.MainCamera).GetComponent<AudioSource>();
            startVolume = audioSource.volume;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.Root().CompareTag(Values.Tags.Player) && currentLevel.LevelMusics.Length > 0)
            {
                if (!isTriggered)
                {
                    
                    StopAllCoroutines();
                    isTriggered = true;
                    StartCoroutine(FadeIn());
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.Root().CompareTag(Values.Tags.Player))
            {
                StopAllCoroutines();
                isTriggered = false;
                StartCoroutine(FadeOut());             
            }          
        }

        private void StopSound()
        {
            audioSource.Stop();
            musicIsPlaying = false;
        }

        private void InitializeSound()
        {
            AudioClip clip;
            clip = currentLevel.LevelMusics[TrackIndex];
            if (clip != null)
            {
                audioSource.clip = clip;
            }
        }

        private IEnumerator FadeOut()
        {
            while (audioSource.volume > 0)
            {
                audioSource.volume -= 1 / AudioFadeTime * Time.deltaTime;   
                yield return null;
            }

            if (!isTriggered)
            {
                StopSound();
            }     
        }

        private IEnumerator FadeIn()
        {
            if (!musicIsPlaying)
            {
                InitializeSound();  
                PlaySound();
                musicIsPlaying = true;
            }


            while (audioSource.volume < 1)
            {
                audioSource.volume += 1 / AudioFadeTime * Time.deltaTime;   
                yield return null;
            }                    
        }

        private void PlaySound()
        {
            audioSource.Play();
        }
    }
}