using UnityEngine;

namespace Game.Entity.Enemies.Attack
{
    class MeleeAttackController : MonoBehaviour
    {
        [SerializeField] private float delayBeforeDestruction;
        
        [Header("Sound")] [SerializeField] private AudioClip meleeSound;
        [SerializeField] private GameObject soundToPlayPrefab;
        private GameObject soundToPlay;
        
        private void Awake()
        {
            CallAttackSound();
            Destroy(this.gameObject, delayBeforeDestruction);
        }

        private void CallAttackSound()
        {
            soundToPlay=Instantiate(soundToPlayPrefab,this.transform.position,Quaternion.identity);
            soundToPlay.GetComponent<AudioManagerSpecificSounds>().Init(meleeSound, true, this.gameObject);
            soundToPlay.GetComponent<AudioManagerSpecificSounds>().PlaySound();
        }

    }
}


