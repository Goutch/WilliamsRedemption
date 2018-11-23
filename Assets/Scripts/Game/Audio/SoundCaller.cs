using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundCaller {

	public static void CallSound(AudioClip sound, GameObject soundToPlayPrefab, GameObject soundOrigin
		, bool shouldSoundStopOnOriginDestruction)
	{
		var soundToPlay=Object.Instantiate(soundToPlayPrefab, soundOrigin.transform.position,Quaternion.identity);
		soundToPlay.GetComponent<AudioManagerSpecificSounds>().Init(sound, shouldSoundStopOnOriginDestruction, soundOrigin);
		soundToPlay.GetComponent<AudioManagerSpecificSounds>().PlaySound();
	} 
}
