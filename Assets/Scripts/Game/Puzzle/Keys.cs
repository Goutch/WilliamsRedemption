using UnityEngine;

namespace Game.Puzzle
{
    public class Keys : MonoBehaviour
    {
        [Tooltip("Door tied to this key.")]
        [SerializeField] private Doors door;


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(Values.Tags.Player))
            {
                door.Unlock();
                gameObject.SetActive(false);
            }
        }
    }
}

