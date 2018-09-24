using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Light
{
    public class LightStimulu : MonoBehaviour
    {
        private List<LightSensor> lightSensorsInRange;
        private List<MovingLightObstacle> movingLightObstaclesInRange;
        private MeshLight meshLight;

        private void Start()
        {
            lightSensorsInRange = new List<LightSensor>();
            movingLightObstaclesInRange = new List<MovingLightObstacle>();

            meshLight = GetComponent<MeshLight>();
        }

        private void Update()
        {
            foreach (LightSensor lightSensor in lightSensorsInRange)
            {
                meshLight.IsWithinLightLimits(lightSensor.transform.position)?.Exposed();
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

                meshLight.HasMovingObstaclesInRange = true;
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
                    meshLight.HasMovingObstaclesInRange = true;
                }
                else
                {
                    meshLight.HasMovingObstaclesInRange = false;
                }
            }
        }

        public void OnRadiusChange(float newRadius)
        {
            GetComponent<CircleCollider2D>().radius = newRadius;
        }

        public void OnDimentionsChange(Vector2 newSize)
        {
            GetComponent<BoxCollider2D>().size = newSize;
            GetComponent<BoxCollider2D>().offset = (newSize / 2) * Vector2.down;
        }
    }
}