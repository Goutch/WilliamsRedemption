using System;
using UnityEngine;

namespace Playmode.Cars.Sounds
{
    [AddComponentMenu("Game/Cars/Sounds/EngineSound")]
    public class EngineSound : MonoBehaviour
    {
        [Header("Engine Sound")] [SerializeField] private AudioClip engineSound;
        [Header("Configuration")] [SerializeField] private float enginePowerBias = 0.2f;
        [SerializeField] private float minPitch = 0.5f;
        [SerializeField] private float maxPitch = 2.5f;
        [SerializeField] private float minVolume = 0.3f;
        [SerializeField] private float maxVolume = 0.5f;

        private Engine engine;
        private AudioSource audioSource;

        private void Awake()
        {
            ValidateSerializedFields();
            InitializeComponent();
        }

        private void ValidateSerializedFields()
        {
            if (engineSound == null)
                throw new ArgumentException("Engine Sound clip must be provided.");
        }

        private void InitializeComponent()
        {
            engine = GetComponent<Engine>();
            audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            audioSource.clip = engineSound;
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
            var enginePower = Mathf.Clamp01(Mathf.Abs(engine.Power) + enginePowerBias);

            audioSource.pitch = Mathf.Lerp(minPitch, maxPitch, enginePower);
            audioSource.volume = Mathf.Lerp(minVolume, maxVolume, enginePower);
        }
    }
}