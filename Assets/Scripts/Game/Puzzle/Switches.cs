using Game.Audio;
using Game.Controller;
using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Puzzle
{
    public class Switches : MonoBehaviour
    {
        [SerializeField] private Sprite unToggledSprite;
        [SerializeField] private Sprite toggledSprite;
        [Tooltip("Objects triggered by this trigger.")] [SerializeField]
        private GameObject[] triggerables;

        [Tooltip("Amount of time before the countdown reaches zero.")] [SerializeField]
        private float shutDownTime;

        [Tooltip("Check this box if you want these objects to use a countdown.")] [SerializeField]
        private bool hasTimer;

        [Tooltip("Check this box if you want to lock the objects after being triggered.")] [SerializeField]
        private bool lockLinkedObjects;

        [Header("Sound")] [SerializeField] private AudioClip switchesSound;
        [SerializeField] private AudioClip timerSound;
        [SerializeField] private GameObject soundToPlayPrefab;
        [SerializeField] private int numberSoundTrackPlayAfterTimer;
        private AudioManagerBackgroundSound audioManagerForTimer;
        
        private float timerStartTime;
        private bool timerHasStarted;
        private SpriteRenderer spriteRenderer;

        private GameController gameController;

        private void Awake()
        {
            audioManagerForTimer = GameObject.FindGameObjectWithTag(Values.Tags.MainCamera)
                .GetComponent<AudioManagerBackgroundSound>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.Root().CompareTag(Values.Tags.Player) && !timerHasStarted)
            {
                SoundCaller.CallSound(switchesSound, soundToPlayPrefab, gameObject, false);
                foreach (var triggerable in triggerables)
                {
                    if (!triggerable.GetComponent<ITriggerable>().IsLocked() &&
                        !triggerable.GetComponent<ITriggerable>().IsOpened())
                    {
                        triggerable.GetComponent<ITriggerable>()?.Open();
                        spriteRenderer.sprite = toggledSprite;
                        if (hasTimer)
                        {
                           
                            triggerable.GetComponent<ITriggerable>()?.Lock();
                            timerStartTime = Time.time;
                            timerHasStarted = true;
                            
                            audioManagerForTimer?.Init(timerSound);
                            audioManagerForTimer?.PlaySound();
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
                        spriteRenderer.sprite = unToggledSprite;
                        if (hasTimer)
                        {                            
                            triggerable.GetComponent<ITriggerable>()?.Lock();
                            timerStartTime = Time.time;
                            timerHasStarted = true;
                            
                            audioManagerForTimer?.Init(timerSound);
                            audioManagerForTimer?.PlaySound();
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
            if (hasTimer)
            {
                gameController = GameObject.FindGameObjectWithTag(Values.Tags.GameController).GetComponent<GameController>();
            }
            spriteRenderer = GetComponent<SpriteRenderer>();
            timerStartTime = 0.0f;
            foreach (var triggerable in triggerables)
            {
                if (triggerable.GetComponent<ITriggerable>().IsOpened())
                {
                    triggerable.GetComponent<ITriggerable>()?.Open();
                    spriteRenderer.sprite = toggledSprite;
                }
                else
                {
                    triggerable.GetComponent<ITriggerable>()?.Close();
                    spriteRenderer.sprite = unToggledSprite;
                }
            }

            timerHasStarted = false;
        }

        private void Update()
        {
            if (hasTimer && timerHasStarted)
            {
                gameController.EventTime = shutDownTime - GetTimeSinceTriggered();
                if (TimeIsUp())
                {
                    ChangeSate();
                    audioManagerForTimer?.TimerSoundStop(numberSoundTrackPlayAfterTimer);
                }
            }
        }

        private bool TimeIsUp()
        {
            if (GetTimeSinceTriggered() >= shutDownTime)
            {
                return true;
            }

            return false;
        }

        private void ChangeSate()
        {
            foreach (var triggerable in triggerables)
            {
                ITriggerable linkedTriggerable = triggerable.GetComponent<ITriggerable>();
                if (linkedTriggerable.IsOpened()&&!linkedTriggerable.StateIsPermanentlyLocked())
                {
                    linkedTriggerable?.Unlock();
                    linkedTriggerable?.Close();
                    spriteRenderer.sprite = unToggledSprite;
                    timerHasStarted = false;
                }
                else if (!linkedTriggerable.IsOpened()&& !linkedTriggerable.StateIsPermanentlyLocked())
                {
                    linkedTriggerable?.Unlock();
                    linkedTriggerable?.Open();
                    spriteRenderer.sprite = toggledSprite;
                    timerHasStarted = false;
                }
            }
        }

        private float GetTimeSinceTriggered()
        {
            return Time.time - timerStartTime;
        }
    }
}