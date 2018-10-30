using UnityEngine;

namespace Game.Puzzle
{
    public class Doors : MonoBehaviour, ITriggerable
    {
        [Tooltip("Locks the door.")]
        [SerializeField] private bool isLocked;
        [Tooltip("Check this box if you want the door to start opened.")]
        [SerializeField] private bool isOpen;


        void Awake()
        {
            if (isOpen)
            {
                Open();
            }
        }

        public void Open()
        {
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            isOpen = true;
        }

        public void Close()
        {
            this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
            isOpen = false;
        }

        public void Lock()
        {
            isLocked = true;
        }

        public void Unlock()
        {
            isLocked = false;
        }

        public bool IsOpened() => isOpen;

        public bool IsLocked() => isLocked;


    }
}

