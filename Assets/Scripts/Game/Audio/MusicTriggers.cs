using Game.Controller;
using UnityEngine;

namespace Game.Audio
{
    public class MusicTriggers : MonoBehaviour
    {
        [SerializeField] private bool isTriggerStartingMusic;
        [SerializeField] private int numberSoundtrackToPlay;

        private AudioManagerBackgroundSound audioManager;

        private AudioClip levelMusic;

        private Level currentLevel;

        private void Awake()
        {
            audioManager = GameObject.FindGameObjectWithTag(Values.Tags.MainCamera)
                .GetComponent<AudioManagerBackgroundSound>();
            currentLevel=GameObject.FindGameObjectWithTag(Values.Tags.GameController)
                .GetComponent<GameController>().CurrentLevel;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.transform.root.CompareTag(Values.Tags.Player))
            {
                if (isTriggerStartingMusic)
                {
                    if (numberSoundtrackToPlay>0 && numberSoundtrackToPlay<=currentLevel.LevelMusics.Length)
                    {
                        InitializeSound();
                    }
                }
                else
                {
                    audioManager.StopSound();
                }
                gameObject.SetActive(false);
            }
        }

        private void InitializeSound()
        {
            AudioClip clip;
            clip = currentLevel.LevelMusics[numberSoundtrackToPlay-1];
            if (clip != null)
            {
                audioManager.Init(clip);
            }
        }
    }
}