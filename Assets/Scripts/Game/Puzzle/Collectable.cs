using Game.Controller;
using UnityEngine;

namespace Game.Puzzle
{
    public class Collectable : MonoBehaviour
    {
        [SerializeField] private int scoreValue;
        
        [Header("Sound")] [SerializeField] private AudioClip collectableSound;
        [SerializeField] private GameObject soundToPlayPrefab;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.transform.root.CompareTag(Values.Tags.Player))
            {
                SoundCaller.CallSound(collectableSound, soundToPlayPrefab, gameObject, false);
                GameObject.FindGameObjectWithTag(Values.Tags.GameController).GetComponent<GameController>().AddCollectable(scoreValue);
                Destroy(this.gameObject);
            }
        }
    }
}