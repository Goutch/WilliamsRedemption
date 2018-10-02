using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Contient nombre de méthodes d'extensions pour les Rigidbody.
    /// </summary>
    public static class RigidbodyExtensions
    {
        /// <summary>
        /// Effectue une translation du Rigidbody. Prends en compte les collisions.
        /// </summary>
        /// <param name="rigidbody">Rigidbody sur lequel appliquer la translation.</param>
        /// <param name="translation">Translation à appliquer.</param>
        public static void Translate(this Rigidbody rigidbody, Vector3 translation)
        {
            rigidbody.MovePosition(rigidbody.position + rigidbody.rotation * translation);
        }

        /// <summary>
        /// Effectue une rotation du Rigidbody. Prends en compte les collisions.
        /// </summary>
        /// <param name="rigidbody">Rigidbody sur lequel appliquer la rotation.</param>
        /// <param name="angle">Angle à appliquer en degrés.</param>
        public static void Rotate(this Rigidbody rigidbody, Vector3 axis, float angle)
        {
            rigidbody.MoveRotation(rigidbody.rotation * Quaternion.Euler(axis.normalized * angle));
        }
    }
}