using System;
using UnityEngine;

namespace Playmode.Cars.Sounds
{
    [AddComponentMenu("Game/Cars/Sounds/WheelSound")]
    public class WheelSound : MonoBehaviour
    {
        [Header("Wheel Sound")] [SerializeField] private AudioClip wheelSkidSound;
        [Header("Configuration")] [SerializeField] private float maxVolume = 0.8f;
        [SerializeField] private float minPitch = 0.8f;
        [SerializeField] private float maxPitch = 1f;

        private Wheel wheel;
        private AudioSource audioSource;

        private void Awake()
        {
            ValidateSerializedFields();
            InitializeComponent();
        }

        private void ValidateSerializedFields()
        {
            if (wheelSkidSound == null)
                throw new ArgumentException("Engine Sound clip must be provided.");
        }

        private void InitializeComponent()
        {
            wheel = GetComponent<Wheel>();
            audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            audioSource.clip = wheelSkidSound;
            audioSource.playOnAwake = true;
            audioSource.loop = true;
            audioSource.Play();
        }

        private void OnDisable()
        {
            audioSource.Stop();
        }

        private void Update()
        {
            var skidClamped = Mathf.Clamp01(wheel.Skid);

            audioSource.pitch = Mathf.Lerp(minPitch, maxPitch, skidClamped);
            audioSource.volume = Mathf.Lerp(0, maxVolume, skidClamped);
        }
    }
}