using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Outil de recherche de chemin basé sur l'algorithme A*.
    /// </summary>
    /// <inheritdoc />
    [AddComponentMenu("Game/Navigation/AStarPathFinder")]
    [ExecuteInEditMode]
    public class AStarPathFinder : PathFinder
    {
        protected override ICollection<Node> GetPath(Node startNode, Node endNode)
        {
            //From each node, which node it can most efficiently be reached from
            var previous = new Dictionary<Node, Node>();
            //Known nodes not yet evaluated
            var openedNodes = new HashSet<Node>();
            //Evaluated nodes
            var closedNodes = new HashSet<Node>();
            //Cost from start node to a specific node
            var costToNode = new Dictionary<Node, float>();
            //Cost from start node to end node, passing by a specific node
            var costToEnd = new Dictionary<Node, float>();

            //Cost to start node is 0
            openedNodes.Add(startNode);
            costToNode[startNode] = 0;
            costToEnd[startNode] = Vector2.Distance(startNode.Position, endNode.Position);

            while (openedNodes.Count > 0)
            {
                var currentNode = GetLeastCostToEndNode(openedNodes, costToEnd);

                if (currentNode == endNode)
                {
                    break; //Path to end found. No need to continue
                }

                openedNodes.Remove(currentNode);
                closedNodes.Add(currentNode);

                var costToCurrentNode = costToNode.ContainsKey(currentNode) ? costToNode[currentNode] : float.MaxValue;

                foreach (var neighbourNode in currentNode.Neighbours)
                {
                    if (!closedNodes.Contains(neighbourNode))
                    {
                        openedNodes.Add(neighbourNode); //openedNodes is "HashSet". Therefore, can't have duplicates

                        var newCostToNeighbour = costToCurrentNode + Vector2.Distance(currentNode.Position,
                                                                                      neighbourNode.Position);

                        var costToNeighbourNode = costToNode.ContainsKey(neighbourNode) ? costToNode[neighbourNode] : float.MaxValue;
                        if (newCostToNeighbour < costToNeighbourNode)
                        {
                            var neighbourCostToEnd = newCostToNeighbour + Vector2.Distance(currentNode.Position,
                                                                                           endNode.Position);

                            previous[neighbourNode] = currentNode;
                            costToNode[neighbourNode] = newCostToNeighbour;
                            costToEnd[neighbourNode] = neighbourCostToEnd;
                        }
                    }
                }
            }

#if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
            {
                debugOpenedNodes = openedNodes;
                debugClosedNodes = closedNodes;
            }
#endif

            return GetPathFromPrevious(previous, endNode);
        }

        private static Node GetLeastCostToEndNode(IEnumerable<Node> openedNodes, IReadOnlyDictionary<Node, float> costToEnd)
        {
            var leastCost = float.MaxValue;
            Node leastCostNode = null;
            foreach (var currentNode in openedNodes)
            {
                var nodeCostToEnd = costToEnd.ContainsKey(currentNode) ? costToEnd[currentNode] : float.MaxValue;
                if (nodeCostToEnd < leastCost)
                {
                    leastCost = nodeCostToEnd;
                    leastCostNode = currentNode;
                }
            }

            return leastCostNode;
        }

        private static ICollection<Node> GetPathFromPrevious(IReadOnlyDictionary<Node, Node> previous, Node endNode)
        {
            IList<Node> path = new List<Node>();

            var currentNode = endNode;
            path.Add(endNode);

            while (previous.ContainsKey(currentNode))
            {
                currentNode = previous[currentNode];
                path.Insert(0, currentNode);
            }

            return path;
        }
    }
}