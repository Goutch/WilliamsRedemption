using Game.Controller;
using UnityEngine;

namespace Game.Puzzle
{
    public class Collectable : MonoBehaviour
    {
        [SerializeField] private int scoreValue;
        [SerializeField] private AudioClip collectableSound;
        [SerializeField] private GameObject soundToPlayPrefab;
        
        private GameObject soundToPlay;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(Values.Tags.Player))
            {
                UseSound();
                GameObject.FindGameObjectWithTag(Values.Tags.GameController).GetComponent<GameController>().AddCollectable(scoreValue);
                Destroy(this.gameObject);
            }
        }
        
        private void UseSound()
        {
            soundToPlay=Instantiate(soundToPlayPrefab,this.transform.position,Quaternion.identity);
            soundToPlay.GetComponent<AudioManagerSpecificSounds>().Init(collectableSound, false, this.gameObject);
            soundToPlay.GetComponent<AudioManagerSpecificSounds>().PlaySound();
        }
    }
}