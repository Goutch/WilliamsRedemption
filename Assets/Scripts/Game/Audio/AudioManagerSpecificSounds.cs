using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Audio
{
    public class AudioManagerSpecificSounds : MonoBehaviour
    {
        [SerializeField] private float soundValue;
    
        private AudioSource source;
        private AudioClip clip;
        private GameObject linkedObject;
        private bool doesSoundStopOnObjectDestroy;
        private bool repeatSound;

        public bool RepeatSound
        {
            get { return repeatSound;}
            set { repeatSound = value; }
        }
        
        
        private void Awake()
        {
            source = GetComponent<AudioSource>();
            repeatSound = false;
        }

        public void Init(AudioClip clip, bool doesSoundStopOnObjectDestroy, GameObject linkedObject)
        {
            this.clip = clip;
            this.doesSoundStopOnObjectDestroy = doesSoundStopOnObjectDestroy;
            this.linkedObject = linkedObject;
        }
        
        

        private void Update()
        {
            if (linkedObject != null)
            {
                transform.position = linkedObject.transform.position;
            }
            else if (linkedObject == null && doesSoundStopOnObjectDestroy)
            {
                Destroy(gameObject);
            }
            else if (repeatSound)
            {
                
            }
            if (!source.isPlaying)
            {
                Destroy(gameObject);
            }
        }

        public void StopSound()
        {
            source.Stop();
        }
        
        public void PlaySound()
        {
            source.PlayOneShot(clip, soundValue);
        }
    }
}
