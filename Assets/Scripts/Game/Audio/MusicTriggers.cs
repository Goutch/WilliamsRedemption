using UnityEngine;

namespace Game.Audio
{
    public class MusicTriggers : MonoBehaviour
    {
        private AudioManagerBackgroundSound audioManager;
        //Valeur temporaire
        private AudioClip levelMusic;
        private bool isMusicPlaying = false;
        
        private void Awake()
        {
            audioManager = GameObject.FindGameObjectWithTag(Values.Tags.MainCamera)
                .GetComponent<AudioManagerBackgroundSound>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!isMusicPlaying)
            {
                audioManager.Init(levelMusic);
                audioManager.PlaySound();
            }
            else
            {
                audioManager.StopSound();
            }
        }
    }
}