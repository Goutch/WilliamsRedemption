using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Outil de recherche de chemin dans un <see cref="NavigationMesh" />.
    /// </summary>
    /// <inheritdoc />
    public abstract class PathFinder : MonoBehaviour
    {
#if UNITY_EDITOR
        [Header("Debug")] [SerializeField] private GameObject debugPathStartPoint;
        [SerializeField] private GameObject debugPathEndPoint;
        [NotNull] [SerializeField] private string gizmoPathPointImageName = "PointPath.png";
        [NotNull] [SerializeField] private string gizmoOpenedPointImageName = "PointVisible.png";
        [NotNull] [SerializeField] private string gizmoClosedPointImageName = "PointInvisible.png";

        private ICollection<Node> debugPath;
        protected ICollection<Node> debugOpenedNodes; //For child classes debug purposes
        protected ICollection<Node> debugClosedNodes; //For child classes debug purposes
#endif
        private NavigationMesh navigationMesh;

        protected void Awake()
        {
            navigationMesh = this.GetComponentInObject<NavigationMesh>();
        }

        /// <summary>
        /// Retourne un chemin entre deux positions dans le <see cref="Graph"/> donné.
        /// </summary>
        /// <param name="start">Position de départ.</param>
        /// <param name="end">Position de fin.</param>
        /// <returns>Chemin vers le point, ou null si aucun chemin n'a pu être trouvé.</returns>
        public ICollection<Node> GetPath(Vector2 start, Vector2 end)
        {
            var graph = navigationMesh.Graph;

            var startNode = graph.Find(start, float.MaxValue); //TODO : Replace float.MaxValue with lower value to make search faster.
            if (startNode == null)
            {
                return null;
            }

            var endNode = graph.Find(end, float.MaxValue);
            if (endNode == null)
            {
                return null;
            }

            return GetPath(startNode, endNode);
        }

        protected abstract ICollection<Node> GetPath(Node startNode, Node endNode);

#if UNITY_EDITOR
        public void ComputeDebugPath()
        {
            if (UnityEditor.EditorApplication.isPlaying)
            {
                throw new Exception("Compute debuging path can only be done while in editor.");
            }

            if (navigationMesh == null)
            {
                navigationMesh = GetComponent<NavigationMesh>(); //Don't use injection in edit mode
            }

            debugPath = GetPath(debugPathStartPoint.transform.position,
                                debugPathEndPoint.transform.position);
        }

        public void ClearDebugPath()
        {
            if (debugPath != null && UnityEditor.EditorApplication.isPlaying)
            {
                throw new Exception("Clear debuging path can only be done while in editor.");
            }

            debugPath = null;
        }

        private void OnDrawGizmosSelected()
        {
            if (debugPath != null && debugOpenedNodes != null)
            {
                ShowDebugPath();
            }
        }

        private void ShowDebugPath()
        {
            Node previousNode = null;
            foreach (var node in debugPath)
            {
                Gizmos.DrawIcon(node.Position, gizmoPathPointImageName);

                if (previousNode != null)
                {
                    Gizmos.DrawLine(previousNode.Position, node.Position);
                }
                previousNode = node;
            }
            foreach (var node in debugOpenedNodes)
            {
                if (!debugPath.Contains(node))
                {
                    Gizmos.DrawIcon(node.Position, gizmoOpenedPointImageName);
                }
            }
            foreach (var node in debugClosedNodes)
            {
                if (!debugPath.Contains(node))
                {
                    Gizmos.DrawIcon(node.Position, gizmoClosedPointImageName);
                }
            }
        }
#endif
    }
}
