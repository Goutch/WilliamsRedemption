using Game.Audio;
using Game.Controller;
using Game.Puzzle;
using Game.Values;
using UnityEngine;
using GameObject = UnityEngine.GameObject;

namespace Game.Entity.Enemies.Boss
{
    public class BossFight : MonoBehaviour
    {
        [SerializeField] private GameObject boss;

        [SerializeField] private Doors doorToCloseOnBossFightBegin;
        [SerializeField] private Doors doorToOpenOnBossDeath;
        [SerializeField] private AudioClip bossMusic;

        private Collider2D bossArea;
        private CameraController cameraController;
        private AudioManagerBackgroundSound audioManager;
        private AudioSource audioSource;

        private void Awake()
        {
            boss.SetActive(false);
            boss.GetComponent<Health>().OnDeath += OnBossDead;
            bossArea = GetComponent<Collider2D>();
            audioManager = GameObject.FindGameObjectWithTag(Values.Tags.MainCamera)
                .GetComponent<AudioManagerBackgroundSound>();

            cameraController = GameObject.FindGameObjectWithTag(Values.Tags.MainCamera)
                .GetComponent<CameraController>();
        }

        private void Start()
        {
            audioSource = GameObject.FindGameObjectWithTag(Values.Tags.MainCamera).GetComponent<AudioSource>();
            
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.isTrigger && other.transform.root.CompareTag(Values.Tags.Player))
            {
                boss.SetActive(true);
    
                UnlockBossDoors();
                CloseBossDoors();
                LockBossDoors();
               
                cameraController.FixPoint(bossArea.bounds.center, bossArea.bounds.size.x / 3);
                
                if (bossMusic != null)
                {
                    audioSource.clip = bossMusic;
                    audioSource.volume = 1;
                    audioSource.Play();
                }
                
                Destroy(GetComponent<BoxCollider2D>());
            }
        }

        private void OnBossDead(GameObject receiver, GameObject attacker)
        {
            UnlockBossDoors();
            OpenBossDoors();
            LockBossDoors();
            audioSource.Stop();
            cameraController.ResumeFollow();
        }

        private void LockBossDoors()
        {
            doorToCloseOnBossFightBegin?.Lock();
            doorToOpenOnBossDeath?.Lock();
        }

        private void OpenBossDoors()
        {
            doorToCloseOnBossFightBegin?.Open();
            doorToOpenOnBossDeath?.Open();
        }

        private void CloseBossDoors()
        {
            doorToCloseOnBossFightBegin?.Close();
            doorToOpenOnBossDeath?.Close();
        }

        private void UnlockBossDoors()
        {
            doorToCloseOnBossFightBegin?.Unlock();
            doorToOpenOnBossDeath?.Unlock();
        }
    }
}