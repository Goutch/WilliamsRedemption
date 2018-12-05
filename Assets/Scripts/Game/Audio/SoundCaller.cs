using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Audio
{
    public static class SoundCaller
    {
        public static void CallSound(AudioClip sound, GameObject soundToPlayPrefab, GameObject soundOrigin
            , bool shouldSoundStopOnOriginDestruction)
        {
            var soundToPlay = Object.Instantiate(soundToPlayPrefab, soundOrigin.transform.position, Quaternion.identity);
            var soundToPlayComponent = soundToPlay.GetComponent<AudioManagerSpecificSounds>();
            soundToPlayComponent.Init(sound, shouldSoundStopOnOriginDestruction, soundOrigin);
            soundToPlayComponent.PlaySound();
        }
    }
}
