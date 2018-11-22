using Harmony;
using UnityEngine;

namespace Game.Puzzle
{
    public class Switches : MonoBehaviour
    {
        [Tooltip("Objects triggered by this trigger.")] [SerializeField]
        private GameObject[] triggerables;

        [Tooltip("Amount of time before the countdown reaches zero.")] [SerializeField]
        private float shutDownTime;

        [Tooltip("Check this box if you want these objects to use a countdown.")] [SerializeField]
        private bool hasTimer;

        [Tooltip("Check this box if you want to lock the objects after being triggered.")] [SerializeField]
        private bool lockLinkedObjects;

        [SerializeField] private AudioClip switchSound;
        [SerializeField] private GameObject soundToPlayPrefab;
        
        private float timerStartTime;
        private bool timerHasStarted;
        private GameObject soundToPlay;


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.Root().CompareTag(Values.Tags.Player) && !timerHasStarted)
            {
                UseSound();
                foreach (var triggerable in triggerables)
                {
                    if (!triggerable.GetComponent<ITriggerable>().IsLocked() &&
                        !triggerable.GetComponent<ITriggerable>().IsOpened())
                    {
                        triggerable.GetComponent<ITriggerable>()?.Open();
                        if (hasTimer)
                        {
                            triggerable.GetComponent<ITriggerable>()?.Lock();
                            timerStartTime = Time.time;
                            timerHasStarted = true;
                        }

                        if (lockLinkedObjects)
                        {
                            triggerable.GetComponent<ITriggerable>().Lock();
                        }
                    }
                    else if (!triggerable.GetComponent<ITriggerable>().IsLocked() &&
                             triggerable.GetComponent<ITriggerable>().IsOpened())
                    {
                        triggerable.GetComponent<ITriggerable>()?.Close();
                        if (hasTimer)
                        {                            
                            triggerable.GetComponent<ITriggerable>()?.Lock();
                            timerStartTime = Time.time;
                            timerHasStarted = true;
                        }

                        if (lockLinkedObjects)
                        {
                            triggerable.GetComponent<ITriggerable>().Lock();
                        }
                    }
                }
            }
            else if(other.Root().CompareTag(Values.Tags.Player) && timerHasStarted)
            {
                timerStartTime = Time.time;
            }
        }

        private void Start()
        {
            timerStartTime = 0.0f;
            foreach (var triggerable in triggerables)
            {
                if (triggerable.GetComponent<ITriggerable>().IsOpened())
                {
                    triggerable.GetComponent<ITriggerable>()?.Open();
                }
                else
                {
                    triggerable.GetComponent<ITriggerable>()?.Close();
                }
            }

            timerHasStarted = false;
        }

        private void Update()
        {
            if (hasTimer && timerHasStarted)
            {
                if (TimeIsUp())
                {
                    ChangeSate();
                }
            }
        }

        private bool TimeIsUp()
        {
            if (Time.time - timerStartTime >= shutDownTime)
            {
                return true;
            }

            return false;
        }

        private void ChangeSate()
        {
            foreach (var triggerable in triggerables)
            {
                if (triggerable.GetComponent<ITriggerable>().IsOpened())
                {
                    triggerable.GetComponent<ITriggerable>()?.Unlock();
                    triggerable.GetComponent<ITriggerable>()?.Close();
                    timerHasStarted = false;
                }
                else if (!triggerable.GetComponent<ITriggerable>().IsOpened())
                {
                    triggerable.GetComponent<ITriggerable>()?.Unlock();
                    triggerable.GetComponent<ITriggerable>()?.Open();
                    timerHasStarted = false;
                }
            }
        }
        
        private void UseSound()
        {
            soundToPlay=Instantiate(soundToPlayPrefab,this.transform.position,Quaternion.identity);
            soundToPlay.GetComponent<AudioManagerSpecificSounds>().Init(switchSound, false, this.gameObject);
            soundToPlay.GetComponent<AudioManagerSpecificSounds>().PlaySound();
        }
    }
}