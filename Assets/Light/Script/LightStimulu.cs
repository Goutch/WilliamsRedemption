using System.Collections.Generic;
using System.Linq;
using Harmony;
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

            meshLight =GetComponent<MeshLight>();
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
            if (other.transform.root.GetComponent<LightSensor>())
            {
                lightSensorsInRange.Add(other.transform.root.GetComponent<LightSensor>());
            }

            if (other.transform.root.GetComponent<MovingLightObstacle>())
            {
                movingLightObstaclesInRange.Add(other.transform.root.GetComponent<MovingLightObstacle>());

                meshLight.HasMovingObstaclesInRange = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.transform.root.GetComponent<LightSensor>())
            {
                lightSensorsInRange.Remove(other.transform.root.GetComponent<LightSensor>());
            }

            if (other.transform.root.GetComponent<MovingLightObstacle>())
            {
                movingLightObstaclesInRange.Remove(other.transform.root.GetComponent<MovingLightObstacle>());

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