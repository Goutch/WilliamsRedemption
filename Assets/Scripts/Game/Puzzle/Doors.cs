using UnityEngine;

namespace Game.Puzzle
{
    public class Doors : MonoBehaviour, ITriggerable
    {
        [Tooltip("Locks the door.")]
        [SerializeField] private bool isLocked;
        [Tooltip("Check this box if you want the door to start opened.")]
        [SerializeField] private bool isOpen;


        //BEN_CORRECTION : Private.
        void Awake()
        {
            if (isOpen)
            {
                Open();
            }
        }

        public void Open()
        {
            //BEN_CORRECTION : Problème de performance : GetComponent devrait être fait qu'une seule
            //                 fois dans le "Awake" (voir Close).
            //BEN_REVIEW : Aussi, "this.gameObject" est redondant et peut être supprimé.
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

        //BEN_REVIEW : Propriétés devraient être en haut du Awake.
        public bool IsOpened() => isOpen;

        public bool IsLocked() => isLocked;


    }
}

