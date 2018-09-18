using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Contient nombre de méthodes d'extensions pour les Gizmos.
    /// </summary>
    public static class GizmosExtensions
    {
        /// <summary>
        /// Draw a line in the scene view.
        /// </summary>
        /// <param name="from">Start of the line.</param>
        /// <param name="to">End of the line.</param>
        public static void DrawLine(Vector2 from, Vector2 to)
        {
            DrawLine(from, to, Color.white);
        }

        /// <summary>
        /// Draw a line in the scene view.
        /// </summary>
        /// <param name="from">Start of the line.</param>
        /// <param name="to">End of the line.</param>
        /// <param name="color">Color of the line</param>
        public static void DrawLine(Vector2 from, Vector2 to, Color color)
        {
            var colorBackup = Gizmos.color;
            Gizmos.color = color;
            Gizmos.DrawLine(from, to);
            Gizmos.color = colorBackup;
        }
        
        /// <summary>
        /// Draw a box in the scene view.
        /// </summary>
        /// <param name="center">Center of the box.</param>
        /// <param name="size">Size of the box.</param>
        public static void DrawBox(Vector2 center, Vector2 size)
        {
            DrawBox(center, size, Quaternion.identity, Color.white);
        }

        /// <summary>
        /// Draw a box in the scene view.
        /// </summary>
        /// <param name="center">Center of the box.</param>
        /// <param name="size">Size of the box.</param>
        /// <param name="color">Color of the lines.</param>
        public static void DrawBox(Vector2 center, Vector2 size, Color color)
        {
            DrawBox(center, size, Quaternion.identity, color);
        }

        /// <summary>
        /// Draw a box in the scene view.
        /// </summary>
        /// <param name="center">Center of the box.</param>
        /// <param name="size">Size of the box.</param>
        /// <param name="rotation">Rotation of the box.</param>
        /// <param name="color">Color of the lines.</param>
        public static void DrawBox(Vector3 center, Vector3 size, Quaternion rotation, Color color)
        {
            var colorBackup = Gizmos.color;
            Gizmos.color = color;

            var point1 = size / 2;
            var point2 = Vector3.Reflect(point1, Vector3.right);
            var point3 = Vector3.Reflect(point2, Vector3.up);
            var point4 = Vector3.Reflect(point3, Vector3.right);
            point1 = rotation * point1 + center;
            point2 = rotation * point2 + center;
            point3 = rotation * point3 + center;
            point4 = rotation * point4 + center;

            Gizmos.DrawLine(point1, point2);
            Gizmos.DrawLine(point2, point3);
            Gizmos.DrawLine(point3, point4);
            Gizmos.DrawLine(point4, point1);

            Gizmos.color = colorBackup;
        }

        /// <summary>
        /// Draw an arrow in the scene view..
        /// </summary>
        /// <param name="from">Start of the arrow.</param>
        /// <param name="to">End of the arrow.</param>
        public static void DrawArrow(Vector3 from, Vector3 to)
        {
            DrawArrow(from, to, Color.white);
        }

        /// <summary>
        /// Draw an arrow in the scene view..
        /// </summary>
        /// <param name="from">Start of the arrow.</param>
        /// <param name="to">End of the arrow.</param>
        /// <param name="color">Color of the arrow.</param>
        public static void DrawArrow(Vector3 from, Vector3 to, Color color)
        {
            var colorBackup = Gizmos.color;
            Gizmos.color = color;

            var direction = to - from;
            var up = Quaternion.LookRotation(direction) * Quaternion.Euler(180 + 30, 0, 0) * Vector3.forward;
            var down = Quaternion.LookRotation(direction) * Quaternion.Euler(180 - 30, 0, 0) * Vector3.forward;
            var left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + 30, 0) * Vector3.forward;
            var right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - 30, 0) * Vector3.forward;

            Gizmos.DrawLine(from, to);
            Gizmos.DrawLine(to, to + up);
            Gizmos.DrawLine(to, to + down);
            Gizmos.DrawLine(to, to + left);
            Gizmos.DrawLine(to, to + right);
            Gizmos.color = colorBackup;
        }
    }
}