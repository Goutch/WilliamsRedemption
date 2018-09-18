using System;
using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Contient nombre de méthodes d'extensions pour l'aléatoire.
    /// </summary>
    public static class RandomExtensions
    {
        /// <summary>
        /// Retourne une position aléatoire entre les bornes données. Les positions négatives sont supportées.
        /// </summary>
        /// <param name="minX">Valeur minimale pour X.</param>
        /// <param name="maxX">Valeur maximale pour X.</param>
        /// <param name="minY">Valeur minimale pour Y.</param>
        /// <param name="maxY">Valeur maximale pour Y.</param>
        /// <returns>Vecteur aléatoire.</returns>
        public static Vector2 GetRandomPosition(float minX, float maxX, float minY, float maxY)
        {
            return new Vector2(GetRandomFloat(minX, maxX), GetRandomFloat(minY, maxY));
        }

        /// <summary>
        /// Retourne une position aléatoire sur les bords d'un rectangle donné.
        /// </summary>
        /// <param name="center">Centre du rectangle.</param>
        /// <param name="height">Hauteur du rectangle.</param>
        /// <param name="width">Largeur du rectangle.</param>
        /// <returns>Position aléatoire sur l'un des bords du rectangle donné.</returns>
        /// <exception cref="System.ArgumentException">Si Height ou Width sont négatifs.</exception>
        public static Vector2 GetRandomPositionOnRectangleEdge(Vector2 center, float height, float width)
        {
            if (height < 0)
            {
                throw new ArgumentException("Rectangle Height cannot be negative.");
            }
            if (width < 0)
            {
                throw new ArgumentException("Rectangle Width cannot be negative.");
            }

            /*
             * Imagine a rectangle, like this :
             * 
             *    |---------------1---------------|
             *    |                               |
             *    |                               |
             *    |                               |
             *    4                               3
             *    |                               |
             *    |                               |
             *    |                               |
             *    |---------------2---------------|
             *    
             * We can "unfold" it in a line, like this :
             * 
             * |--------1--------|--------2--------|--------3--------|--------4--------|
             * 
             * By picking a random position on that line, we pick a random position on the rectangle edges.
             */

            int linePart = Mathf.RoundToInt(GetRandomFloat(0, 1) * 4);

            float randomPosition = GetRandomFloat(-1, 1);
            float x = linePart <= 2 ? randomPosition : (linePart == 3 ? 1 : -1);
            float y = linePart >= 3 ? randomPosition : (linePart == 1 ? 1 : -1);

            return new Vector2(x * (width / 2), y * (height / 2)) + center;
        }

        /// <summary>
        /// Retourne une direction aléatoire. Le vecteur retournée est normalisé et a donc une longueur de 1.
        /// </summary>
        /// <returns>Vecteur représentant une direction aléatoire.</returns>
        public static Vector2 GetRandomDirection()
        {
            return GetRandomPosition(-1, 1, -1, 1).normalized;
        }

        /// <summary>
        /// Retourne 1 ou -1 de manière aléatoire. Il y a 50 % de chance pour chaque option.
        /// </summary>
        /// <returns>1 ou -1</returns>
        public static int GetOneOrMinusOneAtRandom()
        {
            return UnityEngine.Random.value > 0.5f ? 1 : -1;
        }

        /// <summary>
        /// Retourne un nombre entier aléatoire entre deux valeurs.
        /// </summary>
        /// <param name="min">Valeur minimal. Inclusif.</param>
        /// <param name="max">Valeur maximale. Inclusif.</param>
        /// <returns>Nombre aléatoire entre deux valeurs.</returns>
        /// <exception cref="System.ArgumentException">Si Min est plus grand que Max.</exception>
        public static uint GetRandomUInt(uint min, uint max)
        {
            return (uint) GetRandomFloat(min, max);
        }

        /// <summary>
        /// Retourne un nombre entier aléatoire entre deux valeurs. Les valeurs négatives sont supportées.
        /// </summary>
        /// <param name="min">Valeur minimal. Inclusif.</param>
        /// <param name="max">Valeur maximale. Inclusif.</param>
        /// <returns>Nombre aléatoire entre deux valeurs.</returns>
        /// <exception cref="System.ArgumentException">Si Min est plus grand que Max.</exception>
        public static int GetRandomInt(int min, int max)
        {
            return (int) GetRandomFloat(min, max);
        }

        /// <summary>
        /// Retourne un nombre à virgule flotante aléatoire entre deux valeurs. Les valeurs négatives sont supportées.
        /// </summary>
        /// <param name="min">Valeur minimal. Inclusif.</param>
        /// <param name="max">Valeur maximale. Inclusif.</param>
        /// <returns>Nombre aléatoire entre deux valeurs.</returns>
        /// <exception cref="System.ArgumentException">Si Min est plus grand que Max.</exception>
        public static float GetRandomFloat(float min, float max)
        {
            if (min > max)
            {
                throw new ArgumentException("Minimum value must be smaller or equal to maximum value.");
            }
            return UnityEngine.Random.value * (max - min) + min;
        }
    }
}