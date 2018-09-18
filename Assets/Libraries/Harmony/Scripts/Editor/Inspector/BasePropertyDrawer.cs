using UnityEditor;
using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Base pour les <see cref="PropertyDrawer"/>, simplifiant leur implémentation.
    /// </summary>
    /// <typeparam name="T">
    /// Attribut identifiant les propriétés à dessiner par ce <see cref="PropertyDrawer"/>.
    /// </typeparam>
    /// <inheritdoc/>
    public abstract class BasePropertyDrawer<T> : PropertyDrawer where T : BasePropertyAttribute
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            Draw(attribute as T, property, position);
            EditorGUI.EndProperty();
        }

        /// <summary>
        /// Dessine la propriété donnée.
        /// </summary>
        /// <param name="attribute">Attribut attribué à la propriété.</param>
        /// <param name="property">Valeur de la propriété.</param>
        /// <param name="position">Position de la propriété dans l'inspecteur.</param>
        protected abstract void Draw(T attribute, SerializedProperty property, Rect position);
    }
}