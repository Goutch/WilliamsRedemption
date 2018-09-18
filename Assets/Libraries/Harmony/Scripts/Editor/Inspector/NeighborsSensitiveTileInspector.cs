using UnityEditor;

namespace Harmony
{
    /// <summary>
    /// Inspecteur personalisé pour <see cref="NeighborsSensitiveTile"/>.
    /// </summary>
    /// <inheritdoc/>
    [CustomEditor(typeof(NeighborsSensitiveTile), true)]
    public class NeighborsSensitiveTileInspector : BaseInspector
    {
        private GridProperty sprites;

        protected override void Initialize()
        {
            sprites = GetGridProperty("sprites");
        }

        protected override void Draw()
        {
            Initialize();

            DrawPropertyWithLabel(sprites);
        }
    }
}