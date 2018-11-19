using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerBackgroundSound : MonoBehaviour
{
	private AudioSource source;

	private void Awake()
	{
		source = GetComponent<AudioSource>();
	}

	public void PlaySound(AudioClip clip)
	{
		source.PlayOneShot(clip,1F);
	}
}
