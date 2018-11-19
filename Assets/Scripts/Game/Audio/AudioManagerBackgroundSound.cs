
using Game.Entity;
using Game.Entity.Player;
using UnityEngine;

public class AudioManagerBackgroundSound : MonoBehaviour
{
	private AudioSource source;
	private AudioClip clip;
	private bool shouldMusicBePlaying;

	private void Awake()
	{
		source = GetComponent<AudioSource>();
		shouldMusicBePlaying = false;
	}

	private void Start()
	{
		PlayerController.instance.GetComponent<Health>().OnDeath += OnPlayerDie;
	}
	
	public void Init(AudioClip clip)
	{
		this.clip = clip;
		shouldMusicBePlaying = true;
	}

	private void Update()
	{
		if (!source.isPlaying && shouldMusicBePlaying)
		{
			PlaySound();
		}
	}

	private void OnPlayerDie(GameObject gameObject)
	{
		StopSound();
	}
	
	public void PlaySound()
	{
		source.PlayOneShot(clip,1F);
	}

	public void StopSound()
	{
		source.Stop();
		shouldMusicBePlaying = false;
	}
}
