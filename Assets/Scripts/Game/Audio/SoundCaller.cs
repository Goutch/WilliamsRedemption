using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundCaller
{
    //BEN_CORRECTION : Revoir le formatage et l'indentation de ton code en général.
    public static void CallSound(AudioClip sound, GameObject soundToPlayPrefab, GameObject soundOrigin
        , bool shouldSoundStopOnOriginDestruction)
    {
        var soundToPlay = Object.Instantiate(soundToPlayPrefab, soundOrigin.transform.position, Quaternion.identity);
        //BEN_CORRECTION : GetComponent répétés pour le même objet.
        soundToPlay.GetComponent<AudioManagerSpecificSounds>()
            .Init(sound, shouldSoundStopOnOriginDestruction, soundOrigin);
        soundToPlay.GetComponent<AudioManagerSpecificSounds>().PlaySound();
    }
}