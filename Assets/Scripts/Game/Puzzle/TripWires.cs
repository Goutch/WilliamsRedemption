using UnityEngine;

namespace Game.Puzzle
{
    public class TripWires : MonoBehaviour
    {
        [Tooltip("Objects triggered by this trigger.")] [SerializeField]
        private GameObject[] triggerables;

        [Tooltip("Check this box if objects tied to this trigger need to be opened on start")] [SerializeField]
        private bool IsOpened;
        
        [SerializeField] private AudioClip doorSound;
        [SerializeField] private GameObject soundToPlayPrefab;
        
        private GameObject soundToPlay;
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
            if (other.CompareTag(Values.Tags.Player) && !isTripped)
            {
                foreach (var triggerable in triggerables)
                {
                    if (!triggerable.GetComponent<ITriggerable>().IsLocked() &&
                        triggerable.GetComponent<ITriggerable>().IsOpened())
                    {
                        triggerable.GetComponent<ITriggerable>()?.Close();
                        isTripped = true;
                        triggerable.GetComponent<ITriggerable>()?.Lock();
                    }
                    else if (!triggerable.GetComponent<ITriggerable>().IsLocked())
                    {
                        UseSound();
                        triggerable.GetComponent<ITriggerable>()?.Open();
                        isTripped = true;
                        triggerable.GetComponent<ITriggerable>()?.Lock();
                    }
                }
            }
        }
        
        private void UseSound()
        {
            soundToPlay=Instantiate(soundToPlayPrefab,this.transform.position,Quaternion.identity);
            soundToPlay.GetComponent<AudioManagerSpecificSounds>().Init(doorSound, false, this.gameObject);
            soundToPlay.GetComponent<AudioManagerSpecificSounds>().PlaySound();
        }
    }
}