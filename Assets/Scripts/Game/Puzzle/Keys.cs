using UnityEngine;

namespace Game.Puzzle
{
    public class Keys : MonoBehaviour
    {
        [Tooltip("Door tied to this key.")]
        [SerializeField] private Doors door;
        [SerializeField] private AudioClip keySound;
        [SerializeField] private GameObject soundToPlayPrefab;
        
        private GameObject soundToPlay;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(Values.Tags.Player))
            {
                UseSound();
                door.Unlock();
                gameObject.SetActive(false);
            }
        }

        private void UseSound()
        {
            soundToPlay=Instantiate(soundToPlayPrefab,this.transform.position,Quaternion.identity);
            soundToPlay.GetComponent<AudioManagerSpecificSounds>().Init(keySound, false, this.gameObject);
            soundToPlay.GetComponent<AudioManagerSpecificSounds>().PlaySound();
        }
    }
}

