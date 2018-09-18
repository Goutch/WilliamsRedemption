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

        public IEnumerator<Checkpoint> CheckpointLoop()
        {
            var currentCheckpoint = 0;
            while (true)
            {
                yield return Checkpoints[currentCheckpoint];
                currentCheckpoint++;
                currentCheckpoint %= Checkpoints.Length;
            }

            //This is an iterator that loops forever. This is the wanted behavior.
            //ReSharper disable once IteratorNeverReturns
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            Undo.RegisterFullObjectHierarchyUndo(gameObject, "Switch \"Show in Editor\".");
            foreach (var checkpoint in GetComponentsInChildren<Checkpoint>())
            {
                checkpoint.showInEditor = showInEditor;
            }
        }
#endif
    }
}