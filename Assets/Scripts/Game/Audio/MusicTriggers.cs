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

        private void Awake()
        {
            audioManager = GameObject.FindGameObjectWithTag(Values.Tags.MainCamera)
                .GetComponent<AudioManagerBackgroundSound>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.transform.root.CompareTag(Values.Tags.Player))
            {
                if (isTriggerStartingMusic)
                {
                    Level currentLevel=GameObject.FindGameObjectWithTag(Values.Tags.GameController)
                        .GetComponent<GameController>().CurrentLevel;
                    if (numberSoundtrackToPlay>0 && numberSoundtrackToPlay<=currentLevel.NumberSoundTrackPerlevel)
                    {
                        //InitializeSound(numberSoundtrackToPlay);
                    }
                    /*levelMusic = currentLevel.LevelMusic;
                    audioManager.Init(levelMusic);*/
                }
                else
                {
                    audioManager.StopSound();
                }
                gameObject.SetActive(false);
            }
        }

        /*private void InitializeSound(int soundtrackNumber)
        {
            AudioClip clip;
            if (soundtrackNumber == 1)
            {
                clip = GameObject.FindGameObjectWithTag(Values.Tags.GameController)
                    .GetComponent<GameController>().CurrentLevel.LevelMusic1;
            }
            else if(soundtrackNumber == 2)
            {
                clip = GameObject.FindGameObjectWithTag(Values.Tags.GameController)
                    .GetComponent<GameController>().CurrentLevel.LevelMusic2;
            }
            if (clip != null)
            {
                audioManager.Init(clip);
            }
        }*/
    }
}