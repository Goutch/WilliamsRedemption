namespace Harmony
{
    /// <summary>
    /// Indique qu'une propriété devrait être affiché comme un liste de "flags".
    /// </summary>
    /// <seealso cref="FlagsPropertyDrawer"/>
    /// <inheritdoc/>
    public class FlagsFieldAttribute : BasePropertyAttribute
    {
        public FlagsFieldAttribute()
        {
        }

        /// <param name="label">Label à utiliser en guise de nom.</param>
        public FlagsFieldAttribute(string label) : base(label)
        {
        }
    }
}