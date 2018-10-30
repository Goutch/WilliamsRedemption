using UnityEngine;


namespace Game.Entity.Enemies.Boss.Edgar
{
    public delegate void LaserEventHandler(PlasmaLaserController controller);

    public class PlasmaLaserController : MonoBehaviour
    {
        public event LaserEventHandler OnLaserFinish;

        public void LaserFinish()
        {
            OnLaserFinish?.Invoke(this);
            Destroy(this.gameObject);
        }
    }
}

