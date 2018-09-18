using UnityEditor;
using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Affiche la propriété comme une liste de "flags". Une liste de "flags" est une liste de valeur énumérées où
    /// plusieurs valeurs peuvent être cochées en même temps.
    /// </summary>
    /// <remarks>
    /// <para>
    /// L'énumération doit ressembler à ceci :
    /// </para>
    /// <code>[System.Flags]
    /// public enum MyEnum
    /// {
    ///     Flag0 = (1 &lt;&lt; 0),
    ///     Flag1 = (1 &lt;&lt; 1),
    ///     Flag2 = (1 &lt;&lt; 2),
    ///     Flag3 = (1 &lt;&lt; 3),
    /// }
    /// </code>
    /// <para>
    /// Pour les propriétés annotées de <see cref="FlagsFieldAttribute"/> dans un <see cref="MonoBehaviour"/>.
    /// </para>
    /// </remarks>
    /// <inheritdoc/>
    [CustomPropertyDrawer(typeof(FlagsFieldAttribute))]
    public class FlagsPropertyDrawer : BasePropertyDrawer<FlagsFieldAttribute>
    {
        protected override void Draw(FlagsFieldAttribute fieldAttribute, SerializedProperty property, Rect position)
        {
            var currentValue = property.GetEnumValue();
            var newValue = EditorGUI.EnumFlagsField(position, fieldAttribute.Label ?? property.displayName, currentValue);

            property.SetEnumValue(newValue);
        }
    }
}