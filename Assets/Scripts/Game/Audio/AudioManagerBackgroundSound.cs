using Game.Entity;
using Game.Entity.Player;
using UnityEngine;
using Game.Controller;

namespace Game.Audio
{
    public class AudioManagerBackgroundSound : MonoBehaviour
    {
        [SerializeField] private float soundValue;
    
        private AudioSource source;
        private AudioClip clip;
        private bool shouldMusicBePlaying;

        
        public AudioSource GetAudioSource()
        {
            return source;
        }
        
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
            //source.volume = 1;
           // source.PlayOneShot(clip, 1.0f);
            source.Play();
        }

        public void StopSound()
        {
            source.Stop();
            shouldMusicBePlaying = false;
        }

        public void TimerSoundStop(int numberSoundtrackToPlay)
        {
            StopSound();        
            Level currentLevel=GameObject.FindGameObjectWithTag(Game.Values.Tags.GameController)
                .GetComponent<GameController>().CurrentLevel;
            if (currentLevel.LevelMusics.Length >= numberSoundtrackToPlay)
            {
                clip = currentLevel.LevelMusics[numberSoundtrackToPlay-1];
                if (clip != null)
                {
                    Init(clip);
                    PlaySound();
                }
            }
        }


    }
}
