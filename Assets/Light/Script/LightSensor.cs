using UnityEditor.VersionControl;
using UnityEngine;

namespace Light
{
    public delegate void LightSensorEventHandler(bool inlight);

    public class LightSensor : MonoBehaviour
    {
        [SerializeField] private float TimeWithoutLightLimit = 0;
        [SerializeField] private bool inLight;
        public event LightSensorEventHandler OnLightExpositionChange;
        private float lastLightExposure;

        public bool InLight
        {
            get { return inLight; }
            set
            {
                if (inLight != value)
                {
                    inLight = value;
                    if (OnLightExpositionChange != null)
                    {
                        OnLightExpositionChange(value);
                    }
                }
            }
        }

        private void Update()
        {
            transform.Translate(Vector3.down*Time.deltaTime);
            if (lastLightExposure > Time.deltaTime + TimeWithoutLightLimit)
            {
                InLight = false;
            }
        }

        public void Exposed()
        {
            lastLightExposure = Time.time;
            InLight = true;
        }
    }
}