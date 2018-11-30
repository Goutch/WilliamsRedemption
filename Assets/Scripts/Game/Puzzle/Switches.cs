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
        [SerializeField] private GameObject soundToPlayPrefab;
        
        private float timerStartTime;
        private bool timerHasStarted;
        private SpriteRenderer spriteRenderer;

        //BEN_REVIEw : Cette classe a été faite à l'ALPHA, mais je ne l'avais pas vu.
        //
        //             Je me permets donc quelques commentaires :
        //
        //                1. Il y a des "GetComponent" à répétition pour obtenir le même objet au final.
        //                2. Beaucoup de code ici pourrait être remplacé par des "Coroutines".
        
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
                        }

                        if (lockLinkedObjects)
                        {
                            triggerable.GetComponent<ITriggerable>().Lock();
                        }
                    }
                }
            }
            //BEN_REVIEW : IL Y A UN TIMER ICI ?!?!
            //
            //             Sincèrement, cela a été une révélation pour moi, car je l'aurait jamais deviné si j'avais pas vu cela.
            //             Trois choses :
            //
            //                1. Dans votre jeu, les Timers sont bien trop courts.
            //                2. Dans votre jeu, on ne sait pas du tout que c'est une Switch avec un "Timer". Manque de "Feedback".
            //                3. Dans votre code, on ne sait pas que cette "Switch" a un "Timer".
            else if(other.Root().CompareTag(Values.Tags.Player) && timerHasStarted)
            {
                timerStartTime = Time.time;
            }
        }

        private void Start()
        {
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

        //BEN_REVIEW : Typo. "State" au lieu de "Sate".
        private void ChangeSate()
        {
            foreach (var triggerable in triggerables)
            {
                if (triggerable.GetComponent<ITriggerable>().IsOpened())
                {
                    triggerable.GetComponent<ITriggerable>()?.Unlock();
                    triggerable.GetComponent<ITriggerable>()?.Close();
                    spriteRenderer.sprite = unToggledSprite;
                    timerHasStarted = false;
                }
                else if (!triggerable.GetComponent<ITriggerable>().IsOpened())
                {
                    triggerable.GetComponent<ITriggerable>()?.Unlock();
                    triggerable.GetComponent<ITriggerable>()?.Open();
                    spriteRenderer.sprite = toggledSprite;
                    timerHasStarted = false;
                }
            }
        }
    }
}