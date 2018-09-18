using System.Collections.Generic;
using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Représente un noeud (ou point) dans un <see cref="Graph"/>.
    /// </summary>
    /// <remarks>
    /// Un <see cref="Node"/> connait ses voisin, mais ne sait pas où ces derniers sont situés par rapport à 
    /// lui (Nord, Est, Sud, Ouest par exemple).
    /// </remarks>
    public class Node
    {
        private readonly List<Node> neighbours;

        /// <summary>
        /// Position du noeud.
        /// </summary>
        public Vector2 Position { get; }

        /// <summary>
        /// Voisins atteignables en partant de ce noeud.
        /// </summary>
        /// <remarks>
        /// Notez qu'un Node A peut permettre d'atteindre un Node B, mais cela ne signifie pas nécessiarement
        /// que ce même Node B peut permettre d'atteindre le Node A.
        /// </remarks>
        public IEnumerable<Node> Neighbours => neighbours;

        /// <summary>
        /// Créée un nouveau noeud sans voisins.
        /// </summary>
        /// <param name="position">Position du noeud. </param>
        public Node(Vector2 position)
        {
            Position = position;
            neighbours = new List<Node>();
        }

        /// <summary>
        /// Ajoute un voisin à ce noeud.
        /// </summary>
        /// <param name="neighbour">Voisin à ajouter.</param>
        /// <remarks>
        /// <para>
        /// Si le voisin existe déjà, il ne se passe rien.
        /// </para>
        /// </remarks>
        public void AddNeighbour(Node neighbour)
        {
            if (!neighbours.Contains(neighbour))
            {
                neighbours.Add(neighbour);
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Position.ToString();
        }
    }
}