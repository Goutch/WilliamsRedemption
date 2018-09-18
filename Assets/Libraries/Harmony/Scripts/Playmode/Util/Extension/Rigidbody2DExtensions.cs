using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Contient nombre de méthodes d'extensions pour les Rigidbody2D.
    /// </summary>
    public static class Rigidbody2DExtensions
    {
        /// <summary>
        /// Effectue une translation du Rigidbody2D. Prends en compte les collisions.
        /// </summary>
        /// <param name="rigidbody2D">Rigidbody2D sur lequel appliquer la translation.</param>
        /// <param name="translation">Translation à appliquer.</param>
        public static void Translate(this Rigidbody2D rigidbody2D, Vector2 translation)
        {
            rigidbody2D.MovePosition(rigidbody2D.position + translation);
        }

        /// <summary>
        /// Effectue une rotation du Rigidbody2D. Prends en compte les collisions.
        /// </summary>
        /// <param name="rigidbody2D">Rigidbody2D sur lequel appliquer la rotation.</param>
        /// <param name="angle">Angle à appliquer en degrés.</param>
        public static void Rotate(this Rigidbody2D rigidbody2D, float angle)
        {
            rigidbody2D.MoveRotation(rigidbody2D.rotation + angle);
        }
    }
}