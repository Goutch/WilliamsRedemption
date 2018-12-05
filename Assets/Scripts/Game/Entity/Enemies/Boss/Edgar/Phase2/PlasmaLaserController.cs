using UnityEngine;


namespace Game.Entity.Enemies.Boss.Edgar
{
    public delegate void LaserEventHandler(PlasmaLaserController controller);

    public class PlasmaLaserController : MonoBehaviour
    {
        [Header("Sound")] [SerializeField] private AudioClip plasmaLaserSound;
        [SerializeField] private GameObject soundToPlayPrefab;

        public event LaserEventHandler OnLaserFinish;

        public void LaserFinish()
        {
            OnLaserFinish?.Invoke(this);
            Audio.SoundCaller.CallSound(plasmaLaserSound, soundToPlayPrefab, gameObject, false);
            Destroy(this.gameObject);
        }
    }
}