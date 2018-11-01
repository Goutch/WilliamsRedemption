using UnityEngine;

namespace Game.Puzzle.Light
{
    public class RotateLight : MonoBehaviour
    {
        [Tooltip("Angle at which the light will change its direction once reached. (RIGHT)")]
        [SerializeField] private float MaxRightAngle;
        [Tooltip("Angle at which the light will change its direction once reached. (LEFT)")]
        [SerializeField] private float MaxLeftAngle;
        [Tooltip("Speed at which the light rotates.")]
        [SerializeField] private float RotatingSpeed;
        [Tooltip("True if the light is currently rotating towards the right.")]
        [SerializeField] private bool goingRight;

        private CircleLight light;


        // Use this for initialization
        void Start()
        {
            light = gameObject.GetComponent<CircleLight>();
        }

        // Update is called once per frame
        void Update()
        {
            Rotate();
        }

        private void Rotate()
        {
            if (goingRight)
            {
                if (light.FaceAngle < MaxRightAngle)
                {
                    light.FaceAngle += RotatingSpeed * Time.deltaTime;
                }
                else
                {
                    goingRight = false;
                }
            }
            else
            {
                if (light.FaceAngle > MaxLeftAngle)
                {
                    light.FaceAngle -= RotatingSpeed * Time.deltaTime;
                }
                else
                {
                    goingRight = true;
                }
            }
        }
    }

}

