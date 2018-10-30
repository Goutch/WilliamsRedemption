using UnityEngine;

namespace Game.Puzzle.Light
{
    public delegate void LightSensorEventHandler(bool inLight);

    public class LightSensor : MonoBehaviour
    {
        [SerializeField] private float timeWithoutLightLimit;

        public event LightSensorEventHandler OnLightExpositionChange;

        private float lastLightExposure;
        [SerializeField]   private bool inLight;

        public bool InLight
        {
            get { return inLight; }
            set
            {
                if (inLight != value)
                {
                    if(value)
                    {
                        Debug.Log(name+" is now in light");
                    }
                    else
                    {
                        Debug.Log(name+" is now in darkness");
                    }

                    inLight = value;

                    OnLightExpositionChange?.Invoke(value);
                }
            }
        }

        
        private void Update()
        {
            if (Time.time-lastLightExposure > Time.deltaTime + timeWithoutLightLimit)
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