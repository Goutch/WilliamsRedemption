using System;
using System.Reflection;
using UnityEditor;

namespace Harmony
{
    /// <summary>
    /// Contient nombre de méthodes d'extensions pour les SerializedProperty.
    /// </summary>
    public static class SerializedPropertyExtensions
    {
        /// <summary>
        /// Obtient la valeur enuméré d'une propriété, si cette dernière est une propriété d'un type enuméré.
        /// </summary>
        /// <param name="property">SerializedProperty dont la valeur doit être obtenu.</param>
        /// <returns>Valeur de la propriété en tant que type énuméré.</returns>
        public static Enum GetEnumValue(this SerializedProperty property)
        {
            var propertyPathParts = property.propertyPath.Split('.');
            object targetObject = property.serializedObject.targetObject;

            foreach (var path in propertyPathParts)
            {
                // ReSharper disable once PossibleNullReferenceException
                targetObject = targetObject.GetType().GetField(path, BindingFlags.NonPublic |
                                                                     BindingFlags.Public |
                                                                     BindingFlags.Instance).GetValue(targetObject);
            }

            return targetObject as Enum;
        }

        /// <summary>
        /// Modifie la valeur enuméré d'une propriété, si cette dernière est une propriété d'un type enuméré.
        /// </summary>
        /// <param name="property">SerializedProperty dont la valeur doit être obtenu.</param>
        /// <param name="value">Nouvelle valeur de la propriété.</param>
        public static void SetEnumValue(this SerializedProperty property, Enum value)
        {
            property.intValue = (int) Convert.ChangeType(value, property.GetEnumValue().GetType());
        }

        /// <summary>
        /// Indique si une propriété a besoin d'être récréée, car elle n'est plus valide.
        /// </summary>
        /// <param name="property">SerializedProperty à vérifier.</param>
        /// <returns>Vrai si la propriété n'est plus valide et doit être recréée, faux sinon.</returns>
        public static bool NeedRefresh(this SerializedProperty property)
        {
            //If this throws an Exception, the property is invalid.
            try
            {
                var ignore = property.name;
                return false;
            }
            catch (Exception)
            {
                return true;
            }
        }
    }
}