using System;
using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Contient nombre de méthodes d'extensions pour les vecteurs à 2 dimensions de Unity.
    /// </summary>
    public static class Vector2Extensions
    {
        /// <summary>
        /// Indique la distance entre deux points, au carré.
        /// </summary>
        /// <param name="point1">Point 1.</param>
        /// <param name="point2">Point 2.</param>
        /// <returns>Distance entre les deux points, au carré.</returns>
        public static float SqrDistance(Vector2 point1, Vector2 point2)
        {
            return (point2 - point1).sqrMagnitude;
        }

        /// <summary>
        /// Permet de savoir si deux vecteurs de direction sont dans la même direction.
        /// </summary>
        /// <param name="direction1">Point de début de la direction.</param>
        /// <param name="direction2">Point de fin de la direction.</param>
        /// <param name="precision">
        /// Valeur de précision. À 0, l'angle maximal entre les deux vecteurs est de 90. À 1, l'angle maximal entre les deux vecteurs est de 0.
        /// La valeur de précision peut être calculée avec le Cosinus de l'angle maximal (en degrées). La valeur par défaut est de 1.
        /// </param>
        /// <returns>Vrai si "direction1" et "direction2" sont dans la même direction.</returns>
        public static bool IsSameDirection(Vector2 direction1, Vector2 direction2, float precision = 1f)
        {
            if (precision < 0 || precision > 1)
            {
                throw new ArgumentException("Precision must be between 0 and 1, inclusive.");
            }
            return Vector2.Dot(direction1.normalized, direction2.normalized) >= precision;
        }

        /// <summary>
        /// Indique si un point est proche d'un autre point en fonction d'une distance maximale.
        /// </summary>
        /// <param name="point1">Point 1.</param>
        /// <param name="point2">Point 2.</param>
        /// <param name="precision">Distance maximale entre les deux points. Par défaut à 0.</param>
        /// <returns>Vrai si le "point1" est proche du "point2".</returns>
        public static bool IsCloseOf(Vector2 point1, Vector2 point2, float precision = 0f)
        {
            if (precision < 0)
            {
                throw new ArgumentException("Precision must be greater or equal to 0.");
            }
            return SqrDistance(point1, point2) < precision * precision;
        }

        /// <summary>
        /// Permet de savoir si un point est entre deux autres points sur une ligne droite.
        /// </summary>
        /// <param name="point1">Point de début de la ligne.</param>
        /// <param name="point2">Point de fin de la ligne.</param>
        /// <param name="pointToCheck">Point à vérifier.</param>
        /// <param name="precision">
        /// Valeur de précision. À 0, l'angle maximal entre le point et la ligne est de 90. À 1, l'angle maximal entre le point et la ligne est de 0.
        /// La valeur de précision peut être calculée avec le Cosinus de l'angle maximal (en degrées). La valeur par défaut est de 1.
        /// </param>
        /// <returns>Vrai si "pointToCheck" est sur la même ligne que "point1" et "point2" et entre les deux points sur la ligne, faux sinon.</returns>
        public static bool IsPointBetween(Vector2 point1, Vector2 point2, Vector2 pointToCheck, float precision = 1f)
        {
            precision = 1 - precision;

            return Vector2.Dot((point2 - point1).normalized, (pointToCheck - point2).normalized) <= precision &&
                   Vector2.Dot((point1 - point2).normalized, (pointToCheck - point1).normalized) <= precision;
        }
        
        /// <summary>
        /// Permet d'obtenir le point sur une ligne le plus proche d'un autre point.
        /// </summary>
        /// <remarks>
        /// Le point donné est toujours entre "from" et "to".
        /// </remarks>
        /// <param name="from">Position de début de la ligne.</param>
        /// <param name="to">Position de find de la ligne.</param>
        /// <param name="position">Position d'où débuter la recherche.</param>
        /// <returns>Point, sur la ligne entre "from" et "to, le plus proche de "position".</returns>
        public static Vector2 ClosestPointOnLine(Vector2 from, Vector2 to, Vector2 position)
        {
            var directionToLine = position - from;
            var lineDirectionNomalized = (to - from).normalized;
     
            var lineLenght = Vector2.Distance(from, to);
            var travelDistance = Vector2.Dot(directionToLine, lineDirectionNomalized);
     
            if (travelDistance <= 0) return from;
            if (travelDistance >= lineLenght) return to;
     
            var travelDirection = lineDirectionNomalized * travelDistance;
            var closestPoint = from + travelDirection;
     
            return closestPoint;
        }

        /// <summary>
        /// Effectue la rotation d'un point autour d'un pivot avec des angles en degrés.
        /// </summary>
        /// <param name="point">Point ayant à subir la rotation.</param>
        /// <param name="pivot">Pivot de rotation.</param>
        /// <param name="angle">Angle de rotation en degrés sur l'axe des X.</param>
        /// <returns>Nouveau point avec la rotation souhaité.</returns>
        public static Vector2 RotateAround(this Vector2 point, Vector2 pivot, float angle)
        {
            Vector2 pointToRotate = point - pivot;
            angle = Mathf.Deg2Rad * angle;

            float angleCos = Mathf.Cos(angle);
            float angleSin = Mathf.Sin(angle);
            
            pointToRotate = new Vector2(pointToRotate.x * angleCos - pointToRotate.y * angleSin,
                                        pointToRotate.x * angleSin + pointToRotate.y * angleCos);
            
            return pointToRotate + pivot;
        }
    }
}