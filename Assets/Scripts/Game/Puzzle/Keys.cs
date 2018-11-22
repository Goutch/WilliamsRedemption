using UnityEngine;

namespace Game.Puzzle
{
    public class Keys : MonoBehaviour
    {
        [Tooltip("Door tied to this key.")]
        [SerializeField] private Doors door;
        
        [Header("Sound")] [SerializeField] private AudioClip keySound;
        [SerializeField] private GameObject soundToPlayPrefab;
        private GameObject soundToPlay;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.transform.root.CompareTag(Values.Tags.Player))
            {
                CallKeySound();
                door.Unlock();
                Debug.Log("Unlock");
                gameObject.SetActive(false);
            }
        }

        private void CallKeySound()
        {
            soundToPlay=Instantiate(soundToPlayPrefab, transform.position,Quaternion.identity);
            soundToPlay.GetComponent<AudioManagerSpecificSounds>().Init(keySound, false, gameObject);
            soundToPlay.GetComponent<AudioManagerSpecificSounds>().PlaySound();
        }
    }
}

