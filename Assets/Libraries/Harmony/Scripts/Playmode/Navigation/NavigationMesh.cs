using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Outil de génération de graphe de navigation. Ne permet pas de calculer de chemin.
    /// </summary>
    /// <seealso cref="PathFinder"/>
    /// <see cref="AStarPathFinder"/>
    /// <inheritdoc />
    [AddComponentMenu("Game/Navigation/NavigationMesh")]
    [ExecuteInEditMode]
    public class NavigationMesh : MonoBehaviour
    {
        [Header("Configuration")] [SerializeField] private GameObject topLeftLimit;
        [SerializeField] private GameObject bottomRightLimit;
        [SerializeField] private float distanceBetweenNodes = 1f;
        [SerializeField] private float horizontalOffset;
        [SerializeField] private float verticalOffset;
#if UNITY_EDITOR
        [SerializeField] private R.E.Layer[] layers = {R.E.Layer.Default};
#endif

#if UNITY_EDITOR
        [Header("Debug")] [SerializeField] private DebugType show;
        [SerializeField] private int highlightenRegionIndex;
        [NotNull] [SerializeField] private string gizmoVisiblePointImageName = "PointVisible.png";
        [SerializeField] [NotNull] private string gizmoInvisiblePointImageName = "PointInvisible.png";
#endif

        /*
         * Theses next four SerializeField are used to keep the generated graph data.
         */
        [HideInInspector] [SerializeField] private float[] nodesData = { };
        [HideInInspector] [SerializeField] private float[] neighboursData = { };
        [HideInInspector] [SerializeField] private Vector3 topLeft = Vector3.zero;
        [HideInInspector] [SerializeField] private Vector3 bottomRight = Vector3.zero;

        public Graph Graph { get; private set; }

        private void Awake()
        {
            Graph = LoadGraph();
        }

        private Graph LoadGraph()
        {
            var graph = new Graph(topLeft, bottomRight);

            //Load nodes
            for (var i = 0; i < nodesData.Length; i += 2)
            {
                graph.Add(new Node(new Vector2(nodesData[i], nodesData[i + 1])));
            }

            //Load Neighbours
            Node currentNode = null;
            var halfDistanceBetweenNodes = distanceBetweenNodes / 2;
            for (var i = 0; i < neighboursData.Length; i += 4)
            {
                var position = new Vector2(neighboursData[i], neighboursData[i + 1]);
                if (currentNode == null || currentNode.Position != position)
                {
                    currentNode = graph.Find(position, halfDistanceBetweenNodes);
                }

                currentNode.AddNeighbour(graph.Find(new Vector2(neighboursData[i + 2], neighboursData[i + 3]), halfDistanceBetweenNodes));
            }

            return graph;
        }

#if UNITY_EDITOR
        private void SaveGraph(Graph graph)
        {
            var dataAsList = new List<float>(); //Temp data holder

            //Save nodes
            foreach (var node in graph.Nodes)
            {
                dataAsList.Add(node.Position.x);
                dataAsList.Add(node.Position.y);
            }

            nodesData = dataAsList.ToArray();
            dataAsList.Clear();

            //Save Neighbours
            foreach (var node in graph.Nodes)
            {
                foreach (var neighbour in node.Neighbours)
                {
                    dataAsList.Add(node.Position.x);
                    dataAsList.Add(node.Position.y);
                    dataAsList.Add(neighbour.Position.x);
                    dataAsList.Add(neighbour.Position.y);
                }
            }

            neighboursData = dataAsList.ToArray();
            dataAsList.Clear();

            //Save Bounds
            topLeft = topLeftLimit.transform.position;
            bottomRight = bottomRightLimit.transform.position;
        }

        private void LoadGraphIfNeeded()
        {
            if (Graph == null) Graph = LoadGraph();
        }

        public void GenerateNavigationMesh()
        {
            if (EditorApplication.isPlaying)
                throw new Exception("Generation of a NavigationMesh at runtime is not permited. You " +
                                    "can only generate NavigationMeshes while in editor.");

            BeginUndo("Generate Navigation Mesh");

            Graph = new Graph(topLeftLimit.transform.position,
                              bottomRightLimit.transform.position);

            /*
             * Nodes creation
             */
            var leftX = topLeftLimit.transform.position.x + horizontalOffset;
            var rightX = bottomRightLimit.transform.position.x + horizontalOffset;
            var topY = topLeftLimit.transform.position.y + verticalOffset;
            var bottomY = bottomRightLimit.transform.position.y + verticalOffset;

            var layerMask = LayerMask.GetMask(layers.Select(R.S.Layer.ToString).ToArray());

            var currentPosition = new Vector2(leftX, topY);
            while (currentPosition.y >= bottomY) //Going Top Left to Bottom Right
            {
                while (currentPosition.x <= rightX)
                {
                    if (Physics2D.OverlapPoint(currentPosition, layerMask) == null)
                    {
                        Graph.Add(new Node(currentPosition));
                    }

                    currentPosition.x += distanceBetweenNodes;
                }

                currentPosition.x = leftX;
                currentPosition.y -= distanceBetweenNodes;
            }

            /*
             * Neighbours creation
             */
            var halfDistanceBetweenNodes = distanceBetweenNodes / 2;
            foreach (var node in Graph.Nodes)
            {
                var topNeighbour = Graph.Find(new Vector2(node.Position.x,
                                                          node.Position.y + distanceBetweenNodes),
                                              halfDistanceBetweenNodes);
                var bottomNeighbour = Graph.Find(new Vector2(node.Position.x,
                                                             node.Position.y - distanceBetweenNodes),
                                                 halfDistanceBetweenNodes);
                var leftNeighbour = Graph.Find(new Vector2(node.Position.x - distanceBetweenNodes,
                                                           node.Position.y),
                                               halfDistanceBetweenNodes);
                var rightNeighbour = Graph.Find(new Vector2(node.Position.x + distanceBetweenNodes,
                                                            node.Position.y),
                                                halfDistanceBetweenNodes);

                if (topNeighbour != null)
                {
                    node.AddNeighbour(topNeighbour);

                    if (leftNeighbour != null)
                    {
                        var topLeftNeighbour = Graph.Find(new Vector2(node.Position.x - distanceBetweenNodes,
                                                                      node.Position.y + distanceBetweenNodes),
                                                          halfDistanceBetweenNodes);
                        if (topLeftNeighbour != null)
                            node.AddNeighbour(topLeftNeighbour);
                    }

                    if (rightNeighbour != null)
                    {
                        var topRightNeighbour = Graph.Find(new Vector2(node.Position.x + distanceBetweenNodes,
                                                                       node.Position.y + distanceBetweenNodes),
                                                           halfDistanceBetweenNodes);
                        if (topRightNeighbour != null)
                            node.AddNeighbour(topRightNeighbour);
                    }
                }

                if (bottomNeighbour != null)
                {
                    node.AddNeighbour(bottomNeighbour);

                    if (leftNeighbour != null)
                    {
                        var bottomLeftNeighbour = Graph.Find(new Vector2(node.Position.x - distanceBetweenNodes,
                                                                         node.Position.y - distanceBetweenNodes),
                                                             halfDistanceBetweenNodes);
                        if (bottomLeftNeighbour != null)
                            node.AddNeighbour(bottomLeftNeighbour);
                    }

                    if (rightNeighbour != null)
                    {
                        var bottomRightNeighbour = Graph.Find(new Vector2(node.Position.x + distanceBetweenNodes,
                                                                          node.Position.y - distanceBetweenNodes),
                                                              halfDistanceBetweenNodes);
                        if (bottomRightNeighbour != null)
                            node.AddNeighbour(bottomRightNeighbour);
                    }
                }

                if (leftNeighbour != null)
                    node.AddNeighbour(leftNeighbour);

                if (rightNeighbour != null)
                    node.AddNeighbour(rightNeighbour);
            }

            SaveGraph(Graph);
        }

        public void DeleteNavigationMesh()
        {
            BeginUndo("Delete Navigation Mesh");

            Graph = new Graph(topLeftLimit.transform.position,
                              bottomRightLimit.transform.position);

            SaveGraph(Graph);
        }

        private void OnDrawGizmosSelected()
        {
            LoadGraphIfNeeded();

            ShowGraphBounds();

            switch (show)
            {
                case DebugType.Nodes:
                    ShowNodes();
                    break;
                case DebugType.Links:
                    ShowLinks();
                    break;
                case DebugType.NodesAndLinks:
                    ShowNodes();
                    ShowLinks();
                    break;
                case DebugType.Regions:
                    ShowRegions();
                    break;
                case DebugType.HighlightRegion:
                    ShowHighlightenRegion();
                    break;
            }
        }

        private void ShowGraphBounds()
        {
            Gizmos.DrawLine(Graph.TopLeft, Graph.TopRight);
            Gizmos.DrawLine(Graph.BottomLeft, Graph.BottomRight);
            Gizmos.DrawLine(Graph.TopLeft, Graph.BottomLeft);
            Gizmos.DrawLine(Graph.TopRight, Graph.BottomRight);
        }

        private void ShowNodes()
        {
            foreach (var node in Graph.Nodes)
            {
                Gizmos.DrawIcon(node.Position, gizmoVisiblePointImageName);
            }
        }

        private void ShowLinks()
        {
            foreach (var node in Graph.Nodes)
            {
                foreach (var neighbour in node.Neighbours)
                {
                    Gizmos.DrawLine(node.Position, neighbour.Position);
                }
            }
        }

        private void ShowRegions()
        {
            foreach (var region in Graph.Regions)
            {
                Gizmos.DrawLine(region.TopLeft, region.TopRight);
                Gizmos.DrawLine(region.BottomLeft, region.BottomRight);
                Gizmos.DrawLine(region.TopLeft, region.BottomLeft);
                Gizmos.DrawLine(region.TopRight, region.BottomRight);
            }
        }

        private void ShowHighlightenRegion()
        {
            var regionIndex = 0;
            foreach (var region in Graph.Regions)
            {
                if (regionIndex == highlightenRegionIndex)
                {
                    Gizmos.DrawLine(region.TopLeft, region.TopRight);
                    Gizmos.DrawLine(region.BottomLeft, region.BottomRight);
                    Gizmos.DrawLine(region.TopLeft, region.BottomLeft);
                    Gizmos.DrawLine(region.TopRight, region.BottomRight);

                    foreach (var node in region.Nodes)
                    {
                        Gizmos.DrawIcon(node.Position, gizmoVisiblePointImageName);
                    }
                }
                else
                {
                    if (!region.HasSubRegions())
                    {
                        foreach (var node in region.Nodes)
                        {
                            Gizmos.DrawIcon(node.Position, gizmoInvisiblePointImageName);
                        }
                    }
                }

                regionIndex++;
            }
        }

        private void BeginUndo(string taskName)
        {
            Undo.RecordObject(this, taskName);
        }

        private enum DebugType
        {
            // ReSharper disable once UnusedMember.Local
            [UsedByUnity] None,
            Nodes,
            Links,
            NodesAndLinks,
            Regions,
            HighlightRegion
        }
#endif
    }
}