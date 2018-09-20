using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Light
{
    public class LightStimuli : MonoBehaviour
    {
        private List<LightSensor> lightSensorsInRange;
        private MeshLight light;

        private void Start()
        {
            lightSensorsInRange = new List<LightSensor>();
            light = GetComponent<MeshLight>();
        }

        private void Update()
        {

            if (lightSensorsInRange.Any())
            {
                foreach (LightSensor lightSensor in lightSensorsInRange)
                {
                    if (light.isWithinLightLimits(lightSensor.transform.position))
                        lightSensor.Exposed();
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<LightSensor>())
            {
                lightSensorsInRange.Add(other.GetComponent<LightSensor>());
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.GetComponent<LightSensor>())
            {
                lightSensorsInRange.Remove(other.GetComponent<LightSensor>());
            }
        }

        public void OnRadiusChange(float newRadius)
        {
            GetComponent<CircleCollider2D>().radius = newRadius;
        }

        public void OndDimentionsChange(Vector2 newSize)
        {
            GetComponent<BoxCollider2D>().size = newSize;
        }
    }
}