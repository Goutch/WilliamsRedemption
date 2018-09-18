using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Base pour les attributs de propriétés.
    /// </summary>
    public abstract class BasePropertyAttribute : PropertyAttribute
    {
        public string Label { get; }

        protected BasePropertyAttribute()
        {
            Label = null;
        }

        /// <param name="label">Label à utiliser en guise de nom.</param>
        protected BasePropertyAttribute(string label)
        {
            Label = label;
        }
    }
}