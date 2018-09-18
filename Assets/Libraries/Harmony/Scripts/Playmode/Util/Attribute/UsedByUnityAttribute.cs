using System;

namespace Harmony
{
    /// <summary>
    /// Indique que l'élément annotée est appellée par réflexion en dehors du code par Unity.
    /// </summary>
    /// <inheritdoc />
    [AttributeUsage(AttributeTargets.All)]
    public sealed class UsedByUnityAttribute : Attribute
    {
    }
}