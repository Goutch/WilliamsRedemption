using Game.Controller;
using UnityEngine;

namespace Game.Audio
{
    public class MusicTriggers : MonoBehaviour
    {
        [SerializeField] private bool isTriggerStartingMusic;

        private AudioManagerBackgroundSound audioManager;

        private AudioClip levelMusic;
        private bool isMusicPlaying = false;

        private void Awake()
        {
            audioManager = GameObject.FindGameObjectWithTag(Values.Tags.MainCamera)
                .GetComponent<AudioManagerBackgroundSound>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.transform.root.CompareTag(Values.Tags.Player))
            {
                if (!isMusicPlaying)
                {
                    audioManager.Init(GameObject.FindGameObjectWithTag(Values.Tags.GameController)
                        .GetComponent<GameController>().GameMusic);
                }

                audioManager.UpdateMusicTriggers();
                gameObject.SetActive(false);
            }
        }
    }
}