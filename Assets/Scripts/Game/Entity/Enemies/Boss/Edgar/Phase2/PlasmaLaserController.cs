using UnityEngine;


namespace Game.Entity.Enemies.Boss.Edgar
{
    public delegate void LaserEventHandler(PlasmaLaserController controller);

    public class PlasmaLaserController : MonoBehaviour
    {
        [Header("Sound")] [SerializeField] private AudioClip plasmaLaserSound;
        [SerializeField] private GameObject soundToPlayPrefab;
        private GameObject soundToPlay;
        
        public event LaserEventHandler OnLaserFinish;

        public void LaserFinish()
        {
            OnLaserFinish?.Invoke(this);
            CallPlasmaLaserSound();
            Destroy(this.gameObject);
        }
        
        private void CallPlasmaLaserSound()
        {
            soundToPlay = Instantiate(soundToPlayPrefab, transform.position, Quaternion.identity);
            soundToPlay.GetComponent<AudioManagerSpecificSounds>().Init(plasmaLaserSound, false, gameObject);
            soundToPlay.GetComponent<AudioManagerSpecificSounds>().PlaySound();
        }
    }
}

