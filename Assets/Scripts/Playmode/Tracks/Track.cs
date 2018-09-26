using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Playmode.Tracks
{
    [AddComponentMenu("Game/Tracks/Track")]
    public class Track : MonoBehaviour
    {
#if UNITY_EDITOR
        [Header("Debug")] [SerializeField] public bool showInEditor;
#endif
        private Checkpoint[] checkpoints;
        public Checkpoint[] Checkpoints => checkpoints ?? (checkpoints = GetComponentsInChildren<Checkpoint>());
