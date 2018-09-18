using System.Collections.Generic;
using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Représente un ensemble de <see cref="Node"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Un <see cref="Graph"/> ne contient que des informations topologiques. Par exemple, un <see cref="Node"/> connait
    /// ses voisin, mais ne sait pas où ces derniers sont situés par rapport à lui (Nord, Est, Sud, Ouest par exemple).
    /// </para>
    /// <para>
    /// De plus, il ne peux exister plus de 2 <see cref="Node"/> au même endroit dans un <see cref="Graph"/>.
    /// </para>
    /// </remarks>
    public class Graph
    {
        private readonly Region region;

        /// <summary>
        /// Nombre de <see cref="Node"/> dans ce <see cref="Graph"/>.
        /// </summary>
        public int NodeCount => region.NodeCount;

        /// <summary>
        /// Collection de tous les <see cref="Node">Nodes</see> contenus dans ce <see cref="Graph"/>.
        /// </summary>
        public IEnumerable<Node> Nodes => GetNodesEnumerable();

        /// <summary>
        /// Collection de tous les <see cref="Region">Regions</see> de ce <see cref="Graph"/>.
        /// </summary>
        public IEnumerable<Region> Regions => GetRegionsEnumerable();

        /// <summary>
        /// Point haut centre.
        /// </summary>
        public Vector2 TopCenter => region.TopCenter;

        /// <summary>
        /// Point bas centre.
        /// </summary>
        public Vector2 BottomCenter => region.BottomCenter;

        /// <summary>
        /// Point gauche centre.
        /// </summary>
        public Vector2 LeftCenter => region.LeftCenter;

        /// <summary>
        /// Point droite centre.
        /// </summary>
        public Vector2 RightCenter => region.RightCenter;

        /// <summary>
        /// Point haut gauche.
        /// </summary>
        public Vector2 TopLeft => region.TopLeft;

        /// <summary>
        /// Point haut droite.
        /// </summary>
        public Vector2 TopRight => region.TopRight;

        /// <summary>
        /// Point bas gauche.
        /// </summary>
        public Vector2 BottomLeft => region.BottomLeft;

        /// <summary>
        /// Point bas droite.
        /// </summary>
        public Vector2 BottomRight => region.BottomRight;

        /// <summary>
        /// Point central.
        /// </summary>
        public Vector2 Center => region.Center;

        /// <summary>
        /// Taille de la région.
        /// </summary>
        public Vector2 Size => region.Size;

        /// <summary>
        /// Crée un nouveau <see cref="Graph"/> vide.
        /// </summary>
        /// <param name="topLeft">Limite haut gauche.</param>
        /// <param name="bottomRight">Limite bas droite.</param>
        public Graph(Vector2 topLeft, Vector2 bottomRight)
        {
            region = new Region(topLeft, bottomRight);
        }

        /// <summary>
        /// Trouve le <see cref="Node"/> le plus proche d'une position donnée.
        /// </summary>
        /// <param name="position">Position.</param>
        /// <param name="maxDistance">Distance maximale avec la position.</param>
        /// <returns>Node le plus proche de la position donnée.</returns>
        public Node Find(Vector2 position, float maxDistance)
        {
            return region.Find(position, maxDistance);
        }

        /// <summary>
        /// Ajoute un <see cref="Node"/> dans le <see cref="Graph"/>.
        /// </summary>
        /// <param name="node"><see cref="Node"/> à ajouter.</param>
        public void Add(Node node)
        {
            region.Add(node);
        }

        private IEnumerable<Node> GetNodesEnumerable()
        {
            return region.Nodes;
        }

        private IEnumerable<Region> GetRegionsEnumerable()
        {
            yield return region;
            foreach (var subRegion in region.Subregions)
            {
                yield return subRegion;
            }
        }
    }
}