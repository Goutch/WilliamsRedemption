using Game.Controller;
using UnityEngine;

namespace Game.Audio
{
    public class MusicTriggers : MonoBehaviour
    {
        [SerializeField] private bool isTriggerStartingMusic;

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
                    audioManager.Init(GameObject.FindGameObjectWithTag(Values.Tags.GameController)
                        .GetComponent<GameController>().CurrentLevel.LevelMusic);
                }
                else
                {
                    audioManager.StopSound();
                }
                gameObject.SetActive(false);
            }
        }
    }
}