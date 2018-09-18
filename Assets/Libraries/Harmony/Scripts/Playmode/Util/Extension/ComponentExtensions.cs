using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Contient nombre de méthodes d'extensions pour les <see cref="Component">Components</see>.
    /// </summary>
    public static class ComponentExtensions
    {
        /// <summary>
        /// Return the topmost parent GameObject of this component.
        /// </summary>
        /// <param name="component">GameObject to search in.</param>
        /// <returns>Topmost parent GameObject, or his GameObject if it doesn't have a parent.</returns>
        public static GameObject Root(this Component component)
        {
            return component.gameObject.Root();
        }

        /// <summary>
        /// Return the parent GameObject of this component.
        /// </summary>
        /// <param name="component">Component to search in.</param>
        /// <returns>Parent GameObject, or null if it doesn't have a parent.</returns>
        public static GameObject Parent(this Component component)
        {
            return component.gameObject.Parent();
        }

        /// <summary>
        /// Return the childrens of this component's GameObject.
        /// </summary>
        /// <param name="component">Component to search in.</param>
        /// <returns>All the childrens of this component's GameObject.</returns>
        public static GameObject[] Childrens(this Component component)
        {
            return component.gameObject.Childrens();
        }

        /// <summary>
        /// Return the sibblings of this component's GameObject.
        /// </summary>
        /// <param name="component">Component to search in.</param>
        /// <returns>All the sibblings of this component's GameObject.</returns>
        public static GameObject[] Sibblings(this Component component)
        {
            return component.gameObject.Sibblings();
        }

        /// <summary>
        /// Returns the asked component in the component's GameObject, or create it if it doesn't exists.
        /// </summary>
        /// <param name="component">Component to search in.</param>
        /// <typeparam name="T">Type of the component to find.</typeparam>
        /// <returns>omponent found. Nerver null.</returns>
        public static T AddOrGetComponent<T>(this Component component) where T : Component
        {
            return component.gameObject.AddOrGetComponent<T>();
        }
        
        /// <summary>
        /// Returns the asked component in the component's GameObject.
        /// </summary>
        /// <param name="component">Component to search in.</param>
        /// <typeparam name="T">Type of the component to find.</typeparam>
        /// <returns>Component found, or null.</returns>
        public static T GetComponentInObject<T>(this Component component) where T : class
        {
            return component.GetComponent<T>();
        }

        /// <summary>
        /// Returns the asked components in the component's GameObject.
        /// </summary>
        /// <param name="component">Component to search in.</param>
        /// <typeparam name="T">Type of the component to find.</typeparam>
        /// <returns>Components found.</returns>
        public static T[] GetComponentsInObject<T>(this Component component) where T : class
        {
            return component.GetComponents<T>();
        }

        /// <summary>
        /// Returns the asked component in the component's GameObject root.
        /// </summary>
        /// <param name="component">Component to search in.</param>
        /// <typeparam name="T">Type of the component to find.</typeparam>
        /// <returns>Component found, or null.</returns>
        public static T GetComponentInRoot<T>(this Component component) where T : class
        {
            return component.Root().GetComponent<T>();
        }

        /// <summary>
        /// Returns the asked components in the component's GameObject root.
        /// </summary>
        /// <param name="component">Component to search in.</param>
        /// <typeparam name="T">Type of the component to find.</typeparam>
        /// <returns>Components found.</returns>
        public static T[] GetComponentsInRoot<T>(this Component component) where T : class
        {
            return component.Root().GetComponents<T>();
        }

        /// <summary>
        /// Returns the asked component in the component's GameObject root and his childrens.
        /// </summary>
        /// <param name="component">Component to search in.</param>
        /// <typeparam name="T">Type of the component to find.</typeparam>
        /// <returns>Component found, or null.</returns>
        public static T GetComponentInRootChildrens<T>(this Component component) where T : class
        {
            return component.Root().GetComponentInChildren<T>();
        }

        /// <summary>
        /// Returns the asked components in the component's GameObject root and his childrens.
        /// </summary>
        /// <param name="component">Component to search in.</param>
        /// <typeparam name="T">Type of the component to find.</typeparam>
        /// <returns>Components found.</returns>
        public static T[] GetComponentsInRootChildrens<T>(this Component component) where T : class
        {
            return component.Root().GetComponentsInChildren<T>();
        }

        /// <summary>
        /// Returns the asked component in the component's GameObject or his sibblings.
        /// </summary>
        /// <param name="component">Component to search in.</param>
        /// <typeparam name="T">Type of the component to find.</typeparam>
        /// <returns>Component found, or null.</returns>
        public static T GetComponentInSiblings<T>(this Component component) where T : class
        {
            foreach (var sibling in component.Sibblings())
            {
                var componentToGet = sibling.GetComponent<T>();
                if (componentToGet != null)
                {
                    return componentToGet;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns the asked components in the component's GameObject or his sibblings.
        /// </summary>
        /// <param name="component">Component to search in.</param>
        /// <typeparam name="T">Type of the component to find.</typeparam>
        /// <returns>Components found.</returns>
        public static T[] GetComponentsInSiblings<T>(this Component component) where T : class
        {
            var components = new LinkedList<T>();

            foreach (var sibling in component.Sibblings())
            {
                foreach (var componentToGet in sibling.GetComponents<T>())
                {
                    components.AddLast(componentToGet);
                }
            }

            return components.ToArray();
        }

        /// <summary>
        /// Returns the asked component in the first GameObject with the provided tag.
        /// </summary>
        /// <param name="component">Component to search in. Ignored.</param>
        /// <param name="tag">Tag to search.</param>
        /// <typeparam name="T">Type of the component to find.</typeparam>
        /// <returns>
        /// Component found, or null if the GameObject doesn't have the asked component,
        /// or null if no GameObject with this tag exists.
        /// </returns>
        public static T GetComponentInTaggedObject<T>(this Component component, string tag) where T : class
        {
            var taggedGameObject = GameObject.FindGameObjectWithTag(tag);
            if (taggedGameObject == null) return null;

            return taggedGameObject.GetComponent<T>();
        }

        /// <summary>
        /// Returns the asked components in the GameObjects with the provided tag.
        /// </summary>
        /// <param name="component">Component to search in. Ignored.</param>
        /// <param name="tag">Tag to search.</param>
        /// <typeparam name="T">Type of the component to find.</typeparam>
        /// <returns>
        /// Components found.
        /// </returns>
        public static T[] GetComponentsInTaggedObject<T>(this Component component, string tag) where T : class
        {
            var components = new LinkedList<T>();

            foreach (var taggedGameObject in GameObject.FindGameObjectsWithTag(tag))
            {
                foreach (var componentToGet in taggedGameObject.GetComponents<T>())
                {
                    components.AddLast(componentToGet);
                }
            }

            return components.ToArray();
        }

        /// <summary>
        /// Returns the asked components in the first GameObject with the provided tag.
        /// </summary>
        /// <param name="component">Component to search in. Ignored.</param>
        /// <param name="tag">Tag to search.</param>
        /// <typeparam name="T">Type of the component to find.</typeparam>
        /// <returns>Components found, an empty array, or null if no GameObject with this tag exists.</returns>
        public static T GetComponentInTaggedObjectChildrens<T>(this Component component, string tag) where T : class
        {
            var taggedGameObject = GameObject.FindGameObjectWithTag(tag);
            if (taggedGameObject == null) return null;

            return taggedGameObject.GetComponentInChildren<T>();
        }

        /// <summary>
        /// Returns the asked components in the GameObject childrens with the provided tag.
        /// </summary>
        /// <param name="component">Component to search in. Ignored.</param>
        /// <param name="tag">Tag to search.</param>
        /// <typeparam name="T">Type of the component to find.</typeparam>
        /// <returns>Components found.</returns>
        public static T[] GetComponentsInTaggedObjectChildrens<T>(this Component component, string tag) where T : class
        {
            var components = new LinkedList<T>();

            foreach (var taggedGameObject in GameObject.FindGameObjectsWithTag(tag))
            {
                foreach (var componentToGet in taggedGameObject.GetComponentsInChildren<T>())
                {
                    components.AddLast(componentToGet);
                }
            }

            return components.ToArray();
        }
    }
}