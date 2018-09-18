using Harmony;
using UnityEngine;

namespace Playmode.Tracks
{
    [AddComponentMenu("Game/Tracks/Checkpoint")]
    public class Checkpoint : MonoBehaviour
    {
        [SerializeField] private float lenght = 10;
#if UNITY_EDITOR
        [Header("Debug")] [SerializeField] public bool showInEditor;
#endif

        public Vector3 Position => transform.position;
        public Vector3 Direction => transform.right;
        public float Lenght => transform.lossyScale.y * lenght;
        public Vector3 From => Position + transform.up * Lenght;
        public Vector3 To => Position - transform.up * Lenght;

        public bool HasPassedCheckpoint(Vector3 position)
        {
            return Vector3.Dot(position - Position, Direction) > 0;
        }
        
        public Vector3 ClosestPointTo(Vector3 position)
        {
            return Vector2Extensions.ClosestPointOnLine(From, To, position);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (showInEditor)
            {
                GizmosExtensions.DrawLine(From, To, Color.green);
                GizmosExtensions.DrawArrow(Position, Position + Direction * 5, Color.green);
            }
        }
#endif
    }
}