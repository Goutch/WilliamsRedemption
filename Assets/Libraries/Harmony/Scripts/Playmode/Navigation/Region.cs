using System;
using System.Collections.Generic;
using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Représente un un ensemble de <see cref="Node"/> contenus dans une région (comprendre rectangle) en deux dimensions.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Une <see cref="Region"/> est essentiellement un QuadTree, c'est-à-dire une structure de données en arbre. Chaque <see cref="Region"/>
    /// peut contenir des <see cref="Node"/> ou des sous régions. Les quadtrees sont très efficaces pour partitionner un espace 
    /// bidimensionnel en le subdivisant récursivement.
    /// </para>
    /// <para>
    /// <see cref="Region"/> est conçu pour être hautement performant en terme de rapidité et de quantité de mémoire utilisée, mais est surtout 
    /// orienté pour être rapide en lecture. En fait, la recherche d'un <see cref="Node"/> à une position se fait en O(Log n), tandis que 
    /// l'insertion se fait en O(Log n) (et non pas en O(1) comme la plupart des collections).
    /// </para>
    /// <para>
    /// <see cref="Region"/> n'est pas "thread safe" et ne devrait donc pas être utilisée par plusieurs threads en même temps. Si jamais vous 
    /// décidez d'utiliser <see cref="Region"/> dans un contexte multi-thread, vous devez le verouiller avec l'instruction "lock". Prenez note
    /// que vous êtes aussi responsables de verouiller les sous-régions et les <see cref="Node">Nodes</see>. De préférence, utilisez un verrou
    /// global pour une <see cref="Region"/> et ses sous-régions afin d'être plus performant.
    /// </para>
    /// </remarks>
    public class Region
    {
        private const int MaxNodeCount = 4;
        private const int NbSubregions = 4;
        private const int NorthEastRegionIndex = 0;
        private const int NorthWestRegionIndex = 1;
        private const int SouthEastRegionIndex = 2;
        private const int SouthWestRegionIndex = 3;

        private int nodeCount;
        private Node[] nodes;
        private readonly Region[] subregions;

        private readonly Vector2 topLeft;
        private readonly Vector2 bottomRight;
        private readonly Vector2 center;
        private readonly Vector2 size;

        //Reduces memory allocation when, for example, "Find" is called a lot of times.
        //Minimizing memory allocation, and thus garbage collection, greatly improves performances.
        //
        //EX : In a scenario where 12 000 consecutives calls where made to this method.
        //
        //     Before : Alloc -> 0.96 GB, Delay -> 20 seconds
        //     After :  Alloc -> 0.9 MB,  Delay -> 4 miliseconds
        private readonly ReusableList<Node> reusableNodeList;

        /// <summary>
        /// Nombre de <see cref="Node"/> dans cette <see cref="Region"/>.
        /// </summary>
        public int NodeCount => nodeCount;

        /// <summary>
        /// Collection de tous les <see cref="Node">Nodes</see> contenus dans cette <see cref="Region"/>.
        /// </summary>
        public IEnumerable<Node> Nodes => GetNodesEnumerable();

        /// <summary>
        /// Collection de tous les sous-régions contenues dans cette <see cref="Region"/>.
        /// </summary>
        public IEnumerable<Region> Subregions => GetRegionsEnumerable();

        /// <summary>
        /// Point haut centre.
        /// </summary>
        public Vector2 TopCenter => new Vector2(center.x, topLeft.y);

        /// <summary>
        /// Point bas centre.
        /// </summary>
        public Vector2 BottomCenter => new Vector2(center.x, bottomRight.y);

        /// <summary>
        /// Point gauche centre.
        /// </summary>
        public Vector2 LeftCenter => new Vector2(topLeft.x, center.y);

        /// <summary>
        /// Point droite centre.
        /// </summary>
        public Vector2 RightCenter => new Vector2(bottomRight.x, center.y);

        /// <summary>
        /// Point haut gauche.
        /// </summary>
        public Vector2 TopLeft => topLeft;

        /// <summary>
        /// Point haut droite.
        /// </summary>
        public Vector2 TopRight => new Vector2(bottomRight.x, topLeft.y);

        /// <summary>
        /// Point bas gauche.
        /// </summary>
        public Vector2 BottomLeft => new Vector2(topLeft.x, bottomRight.y);

        /// <summary>
        /// Point bas droite.
        /// </summary>
        public Vector2 BottomRight => bottomRight;

        /// <summary>
        /// Point central.
        /// </summary>
        public Vector2 Center => center;

        /// <summary>
        /// Taille de la région.
        /// </summary>
        public Vector2 Size => size;

        private Region NorthEast => subregions[NorthEastRegionIndex] ?? (subregions[NorthEastRegionIndex] = new Region(TopCenter, RightCenter));
        private Region NorthWest => subregions[NorthWestRegionIndex] ?? (subregions[NorthWestRegionIndex] = new Region(TopLeft, Center));
        private Region SouthEast => subregions[SouthEastRegionIndex] ?? (subregions[SouthEastRegionIndex] = new Region(Center, BottomRight));
        private Region SouthWest => subregions[SouthWestRegionIndex] ?? (subregions[SouthWestRegionIndex] = new Region(LeftCenter, BottomCenter));

        /// <summary>
        /// Crée une nouvelle <see cref="Region"/> vide.
        /// </summary>
        /// <param name="topLeft">Limite haut gauche.</param>
        /// <param name="bottomRight">Limite bas droite.</param>
        public Region(Vector2 topLeft, Vector2 bottomRight)
        {
            if (topLeft.x > bottomRight.x || topLeft.y < bottomRight.y)
            {
                throw new ArgumentException("Unable to create region : top left limit is below or to the right of the bottom right limit.");
            }

            nodeCount = 0;
            nodes = new Node[MaxNodeCount];
            subregions = new Region[NbSubregions];

            this.topLeft = topLeft;
            this.bottomRight = bottomRight;
            size = new Vector2(bottomRight.x - topLeft.x, topLeft.y - bottomRight.y);
            center = new Vector2(bottomRight.x - size.x / 2, topLeft.y - size.y / 2);

            reusableNodeList = new ReusableList<Node>();
        }

        /// <summary>
        /// Indique si la <see cref="Region"/> contient des sous-régions.
        /// </summary>
        /// <returns>Vrai si la <see cref="Region"/> contient des sous-régions, faux sinon.</returns>
        public bool HasSubRegions()
        {
            return nodeCount > MaxNodeCount; //Once there's more than "MaxNodeCount" nodes, there's at least one subregion.
        }

        /// <summary>
        /// Indique si la <see cref="Region"/> entoure la position donnée.
        /// </summary>
        /// <param name="position">Position.</param>
        /// <returns>Vrai si la <see cref="Region"/> entoure la position donnée, faux sinon.</returns>
        /// <remarks>
        /// <b>Attention!</b> N'indique pas si la <see cref="Region"/> contient un <see cref="Node"/> à cette positon, mais bien si la 
        /// <see cref="Region"/> entoure la position.
        /// </remarks>
        public bool Encloses(Vector2 position)
        {
            return position.x >= topLeft.x && position.x <= bottomRight.x &&
                   position.y <= topLeft.y && position.y >= bottomRight.y;
        }

        /// <summary>
        /// Trouve le <see cref="Node"/> le plus proche d'une position donnée.
        /// </summary>
        /// <param name="position">Position.</param>
        /// <param name="maxDistance">Distance maximale avec la position (voir remarques). Par défaut à "float.MaxValue".</param>
        /// <returns><see cref="Node"/> le plus proche de la position donnée, ou null si aucune <see cref="Node"/> n'a été trouvée.</returns>
        /// <remarks>
        /// <para>
        /// Pour des questions de performances, la distance est calculée pas comme un cercle, mais comme un carré.
        /// </para>
        /// <para>
        /// Ex :
        /// </para>
        /// <code>
        ///         *  * 
        ///      *        * 
        ///     *          *
        ///     *          *
        ///      *        * 
        ///         *  *
        /// 
        /// VS
        /// 
        ///     +----------+
        ///     |          |
        ///     |          |
        ///     |          |
        ///     |          |
        ///     +----------+
        /// </code>
        /// </remarks>
        public Node Find(Vector2 position, float maxDistance)
        {
            return reusableNodeList.Use(candidates =>
            {
                FindAllCandidatesNoAlloc(position, maxDistance, candidates);
                return FindNearestCandidateNoAlloc(position, maxDistance, candidates);
            });
        }

        /// <summary>
        /// Ajoute un <see cref="Node"/> dans la <see cref="Region"/>.
        /// </summary>
        /// <param name="node"><see cref="Node"/> à ajouter.</param>
        public void Add(Node node)
        {
            if (!Encloses(node.Position))
            {
                throw new ArgumentException("Node cannot be added into region : it doesn't encloses the node position.");
            }

            if (!IsFull())
            {
                AddIntoRegion(node);
            }
            else
            {
                MigrateIntoSubregions();

                AddIntoSubregions(node);
            }

            nodeCount++;
        }

        private bool IsFull()
        {
            return nodeCount >= MaxNodeCount;
        }

        private void AddIntoRegion(Node node)
        {
            if (IsFull())
            {
                throw new ArgumentException("Cannot add node into region : it's full.");
            }

            nodes[nodeCount] = node;
        }

        private void AddIntoSubregions(Node node)
        {
            if (!IsFull())
            {
                throw new ArgumentException("Cannot add node into subregions : there's still space in current region.");
            }

            GetEnclosingSubregion(node.Position).Add(node);
        }

        private void MigrateIntoSubregions()
        {
            if (!IsFull())
            {
                throw new ArgumentException("Cannot migrate nodes into subregions : there's still space in current region.");
            }

            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    AddIntoSubregions(node);
                }

                nodes = null;
            }
        }

        private Region GetEnclosingSubregion(Vector2 position)
        {
            //Note : When inserting in subregions, we first find which subregion encloses the node position. Because subregions overlap
            //       on their edges, there will be times where two subregions enclose the same position.
            //
            //       Therefore, when we add a node in subregions, we must traverse the sub-regions in the same order as when we perform a 
            //       node search.
            if (position.y >= center.y)
            {
                return position.x >= center.x ? NorthEast : NorthWest;
            }
            else
            {
                return position.x >= center.x ? SouthEast : SouthWest;
            }
        }

        private bool IntersectWithCircle(Vector2 position, float radius)
        {
            //Find the point of the rectangle that is the closest to the circle's center.
            //Check if that point is in the circle.
            var distanceX = position.x - Mathf.Max(topLeft.x, Mathf.Min(position.x, topLeft.x + size.x));
            var distanceY = position.y - Mathf.Max(bottomRight.y, Mathf.Min(position.y, bottomRight.y + size.y));
            return distanceX * distanceX + distanceY * distanceY < radius * radius;
        }

        private void FindAllCandidatesNoAlloc(Vector2 position, float maxDistance, IList<Node> candidates)
        {
            if (IntersectWithCircle(position, maxDistance))
            {
                if (HasSubRegions())
                {
                    foreach (var subregion in subregions)
                    {
                        subregion?.FindAllCandidatesNoAlloc(position, maxDistance, candidates);
                    }
                }
                else
                {
                    var maxDistanceSquared = maxDistance * maxDistance;
                    foreach (var node in nodes)
                    {
                        if (node != null)
                        {
                            var distanceSquared = Vector2Extensions.SqrDistance(position, node.Position);
                            if (distanceSquared < maxDistanceSquared)
                            {
                                candidates.Add(node);
                            }
                        }
                    }
                }
            }
        }

        private Node FindNearestCandidateNoAlloc(Vector2 position, float maxDistance, IList<Node> candidates)
        {
            Node nearestNode = null;
            var maxDistanceSquared = maxDistance * maxDistance;
            var nearestDistanceSquared = float.MaxValue;
            foreach (var node in candidates)
            {
                if (node != null)
                {
                    var distanceSquared = Vector2Extensions.SqrDistance(position, node.Position);
                    if (distanceSquared < maxDistanceSquared && distanceSquared < nearestDistanceSquared)
                    {
                        nearestNode = node;
                        nearestDistanceSquared = distanceSquared;
                    }
                }
            }

            return nearestNode;
        }

        private IEnumerable<Node> GetNodesEnumerable()
        {
            if (HasSubRegions())
            {
                foreach (var subregion in subregions)
                {
                    if (subregion != null)
                    {
                        foreach (var subNode in subregion.Nodes)
                        {
                            yield return subNode;
                        }
                    }
                }
            }
            else
            {
                foreach (var node in nodes)
                {
                    if (node != null)
                    {
                        yield return node;
                    }
                }
            }
        }

        private IEnumerable<Region> GetRegionsEnumerable()
        {
            foreach (var subregion in subregions)
            {
                if (subregion != null)
                {
                    yield return subregion;

                    foreach (var subSubregion in subregion.Subregions)
                    {
                        yield return subSubregion;
                    }
                }
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"Center : {Center}, Size : {Size}";
        }
    }
}