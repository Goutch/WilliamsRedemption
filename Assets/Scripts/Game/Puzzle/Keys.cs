using UnityEngine;

namespace Game.Puzzle
{
    public class Keys : MonoBehaviour
    {
        [Tooltip("Door tied to this key.")]
        [SerializeField] private Doors door;
        [SerializeField] private AudioClip keySound;

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
            GameObject.FindGameObjectWithTag(Values.Tags.GameController).GetComponent<AudioManager>()
                .PlaySound(keySound);
        }
    }
}

