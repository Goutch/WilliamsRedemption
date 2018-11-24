using UnityEngine;

namespace Game.Puzzle
{
    public class Keys : MonoBehaviour
    {
        [Tooltip("Door tied to this key.")] [SerializeField]
        private Doors door;

        [Header("Sound")] [SerializeField] private AudioClip keySound;
        [SerializeField] private GameObject soundToPlayPrefab;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.transform.root.CompareTag(Values.Tags.Player))
            {
                SoundCaller.CallSound(keySound, soundToPlayPrefab, gameObject, false);
                door.Unlock();
                Debug.Log("Unlock");
                gameObject.SetActive(false);
            }
        }
    }
}