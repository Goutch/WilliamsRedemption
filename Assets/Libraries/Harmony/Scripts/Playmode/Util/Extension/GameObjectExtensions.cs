using System;
using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Contient nombre de méthodes d'extensions pour les GameObjects.
    /// </summary>
    public static class GameObjectExtensions
    {
        /// <summary>
        /// Return the topmost parent of this GameObject.
        /// </summary>
        /// <param name="gameObject">GameObject to search in.</param>
        /// <returns>Topmost parent GameObject, or ifself if it doesn't have a parent.</returns>
        public static GameObject Root(this GameObject gameObject)
        {
            return gameObject.transform.root.gameObject;
        }

        /// <summary>
        /// Return the parent of this GameObject.
        /// </summary>
        /// <param name="gameObject">GameObject to search in.</param>
        /// <returns>Parent GameObject, or null if it doesn't have a parent.</returns>
        public static GameObject Parent(this GameObject gameObject)
        {
            var parentTransform = gameObject.transform.parent;
            return parentTransform == null ? null : parentTransform.gameObject;
        }

        /// <summary>
        /// Return the childrens of this GameObject.
        /// </summary>
        /// <param name="gameObject">GameObject to search in.</param>
        /// <returns>All the childrens of this GameObject.</returns>
        public static GameObject[] Childrens(this GameObject gameObject)
        {
            var transform = gameObject.transform;
            var childCount = transform.childCount;

            var childrens = new GameObject[childCount];
            for (var i = 0; i < childCount; i++)
            {
                childrens[i] = transform.GetChild(i).gameObject;
            }

            return childrens;
        }

        /// <summary>
        /// Return the sibblings of this GameObject.
        /// </summary>
        /// <param name="gameObject">GameObject to search in.</param>
        /// <returns>All the sibblings of this GameObject.</returns>
        public static GameObject[] Sibblings(this GameObject gameObject)
        {
            var parent = gameObject.Parent();
            return parent == null ? gameObject.scene.GetRootGameObjects() : parent.Childrens();
        }
        
        /// <summary>
        /// Returns the asked component in the GameObject, or create it if it doesn't exists.
        /// </summary>
        /// <param name="gameObject">GameObject to get the component from.</param>
        /// <typeparam name="T">Type of the component to find.</typeparam>
        /// <returns>omponent found. Nerver null.</returns>
        public static T AddOrGetComponent<T>(this GameObject gameObject) where T : Component
        {
            var component = gameObject.GetComponent<T>();
            if (component == null) component = gameObject.AddComponent<T>();
            return component;
        }
        
    }
}