using Game.Audio;
using UnityEngine;

namespace Game.Puzzle
{
    public class TripWires : MonoBehaviour
    {
        [Tooltip("Objects triggered by this trigger.")] [SerializeField]
        private GameObject[] triggerables;

        [Tooltip("Check this box if objects tied to this trigger need to be opened on start")] [SerializeField]
        private bool IsOpened;

        [SerializeField]private bool PermanentlyLockTriggerable;
        
        [Header("Sound")] [SerializeField] private AudioClip doorSound;
        [SerializeField] private GameObject soundToPlayPrefab;
        
        private bool isTripped;

        private void Awake()
        {
            isTripped = false;

            if (IsOpened)
            {
                foreach (var triggerable in triggerables)
                {
                    triggerable.GetComponent<ITriggerable>()?.Open();
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.transform.root.CompareTag(Values.Tags.Player) && !isTripped)
            {
                foreach (var triggerable in triggerables)
                {
                    ITriggerable linkedTriggerable = triggerable.GetComponent<ITriggerable>();
                  
                    if (!linkedTriggerable.IsLocked() &&
                        linkedTriggerable.IsOpened())
                    {
                        linkedTriggerable?.Close();
                        isTripped = true;
                        linkedTriggerable?.Lock();
                    }
                    else if (!linkedTriggerable.IsLocked())
                    {
                        SoundCaller.CallSound(doorSound, soundToPlayPrefab, gameObject, false);
                        linkedTriggerable?.Open();
                        isTripped = true;
                        linkedTriggerable?.Lock();
                    }
                    else
                    {
                        linkedTriggerable.PermanentlyLock();
                    }
                }
            }
        }

    }
}