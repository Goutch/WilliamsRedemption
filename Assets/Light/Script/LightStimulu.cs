using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Light
{
    public class LightStimulu : MonoBehaviour
    {
        private List<LightSensor> lightSensorsInRange;
        private List<MovingLightObstacle> movingLightObstaclesInRange;
        private MeshLight light;

        private void Start()
        {
            lightSensorsInRange = new List<LightSensor>();
            movingLightObstaclesInRange = new List<MovingLightObstacle>();
            light = GetComponent<MeshLight>();
        }

        private void Update()
        {
            if (lightSensorsInRange.Any())
            {
                foreach (LightSensor lightSensor in lightSensorsInRange)
                {
                    light.IsWithinLightLimits(lightSensor.transform.position)?.Exposed();
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<LightSensor>())
            {
                lightSensorsInRange.Add(other.GetComponent<LightSensor>());
            }

            if (other.GetComponent<MovingLightObstacle>())
            {
                movingLightObstaclesInRange.Add(other.GetComponent<MovingLightObstacle>());

                if (movingLightObstaclesInRange.Any())
                {
                    light.HasMovingObstaclesInRange = true;
                }
                else
                {
                    light.HasMovingObstaclesInRange = false;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.GetComponent<LightSensor>())
            {
                lightSensorsInRange.Remove(other.GetComponent<LightSensor>());
            }

            if (other.GetComponent<MovingLightObstacle>())
            {
                movingLightObstaclesInRange.Remove(other.GetComponent<MovingLightObstacle>());
                if (movingLightObstaclesInRange.Any())
                {
                    light.HasMovingObstaclesInRange = true;
                }
                else
                {
                    light.HasMovingObstaclesInRange = false;
                }
            }
        }

        public void OnRadiusChange(float newRadius)
        {
            GetComponent<CircleCollider2D>().radius = newRadius;
        }

        public void OndDimentionsChange(Vector2 newSize)
        {
            GetComponent<BoxCollider2D>().size = newSize;
            GetComponent<BoxCollider2D>().offset = (newSize / 2) * Vector2.down;
        }
    }
}