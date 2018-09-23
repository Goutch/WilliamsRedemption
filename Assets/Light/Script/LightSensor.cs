using System;
using System.CodeDom;
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

        private void Awake()
        {
            if (!GetComponent<Collider2D>())
            {
                throw new Exception("A lightSensor need a collider2D");
            }

            if (!GetComponent<Rigidbody2D>())
            {
                throw new Exception("A lightSensor need a RigidBody2D");
            }
        }
        
        private void Update()
        {
            if (Time.time-lastLightExposure > Time.deltaTime + TimeWithoutLightLimit)
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