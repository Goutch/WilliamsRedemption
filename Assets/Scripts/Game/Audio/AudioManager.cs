using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public void PlaySound(AudioSource source, AudioClip clip)
	{
		source.PlayOneShot(clip,1F);
	}
}
