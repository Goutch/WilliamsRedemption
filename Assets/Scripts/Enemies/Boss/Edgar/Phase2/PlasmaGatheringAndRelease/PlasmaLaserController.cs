using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Edgar
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

