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
            audioSource = audioManager.GetAudioSource();
            startVolume = audioSource.volume;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.Root().CompareTag(Values.Tags.Player) && currentLevel.LevelMusics.Length > 0)
            {
                if (!isTriggered)
                {
                    
                    StopCoroutine(FadeOut());
                    isTriggered = true;
                    StartCoroutine(FadeIn());
//                    if (musicIsPlaying)
//                    {
//                        StopCoroutine(FadeOut());
//                        StartCoroutine(FadeIn());
//                        
//                    }
//                    else
//                    {
//                        StartCoroutine(FadeIn());  
//                    }
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.Root().CompareTag(Values.Tags.Player))
            {
                StopCoroutine(FadeIn());
                isTriggered = false;
                StartCoroutine(FadeOut());             
            }          
        }

        private void StopSound()
        {
            audioManager.StopSound();
            musicIsPlaying = false;
        }

        private void InitializeSound()
        {
            AudioClip clip;
            clip = currentLevel.LevelMusics[TrackIndex];
            if (clip != null)
            {
                audioManager.Init(clip);
            }
        }

        private IEnumerator FadeOut()
        {
            while (audioSource.volume > 0)
            {
                audioSource.volume -= startVolume / AudioFadeTime * Time.deltaTime;   
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
                musicIsPlaying = true;
            }
            else
            {
                
            }

            while (audioSource.volume < 1)
            {
                audioSource.volume += startVolume / AudioFadeTime * Time.deltaTime;   
                yield return null;
            }                    
        }
    }
}