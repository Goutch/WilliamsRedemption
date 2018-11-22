using UnityEngine;

namespace Game.Entity.Enemies.Attack
{
    class MeleeAttackController : MonoBehaviour
    {
        [SerializeField] private float delayBeforeDestruction;
        [SerializeField] private AudioClip meleeSound;
        [SerializeField] private GameObject soundToPlayPrefab;
        
        private GameObject soundToPlay;
        

        private void Awake()
        {
            UseSound();
            Destroy(this.gameObject, delayBeforeDestruction);
        }

        private void UseSound()
        {
            soundToPlay=Instantiate(soundToPlayPrefab,this.transform.position,Quaternion.identity);
            soundToPlay.GetComponent<AudioManagerSpecificSounds>().Init(meleeSound, true, this.gameObject);
            soundToPlay.GetComponent<AudioManagerSpecificSounds>().PlaySound();
        }

    }
}


