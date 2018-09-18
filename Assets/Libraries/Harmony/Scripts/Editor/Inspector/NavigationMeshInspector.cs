using UnityEditor;

namespace Harmony
{
    /// <summary>
    /// Inspecteur personalisé pour <see cref="NavigationMesh"/>.
    /// </summary>
    /// <inheritdoc/>
    [CustomEditor(typeof(NavigationMesh), true)]
    public class NavigationMeshInspector : BaseInspector
    {
        private NavigationMesh navigationMesh;

        protected override void Initialize()
        {
            navigationMesh = target as NavigationMesh;
        }

        protected override void Draw()
        {
            Initialize();
            DrawDefault();

            BeginTable();
            BeginTableCol();
            DrawTitleLabel("Navigation Mesh utils");

            if (!GetBasicProperty("topLeftLimit").IsNull() && !GetBasicProperty("bottomRightLimit").IsNull())
            {
                DrawButton("Generate Navigation Mesh", GenerateNavigationMesh);
                DrawButton("Delete Navigation Mesh", DeleteNavigationMesh);
            }
            else
            {
                DrawErrorBox("You must set the limits before you can generate the navigation mesh.");
            }

            EndTableCol();
            EndTable();

            if (navigationMesh.Graph != null)
            {
                BeginTable();
                BeginTableCol();
                DrawTitleLabel("Navigation Mesh Statistics");

                DrawLabel("Node court : " + navigationMesh.Graph.NodeCount);

                EndTableCol();
                EndTable();
            }
            
            DrawInfoBox("You might want to play a little with the positions of the limits to improve the coverage. " +
                        "For example, move the top left limit up by half a unit and then left by the same amount.\n\n" +
                        "In a grid, place your limits in the middle of the cells.");
        }

        private void GenerateNavigationMesh()
        {
            navigationMesh.GenerateNavigationMesh();
        }

        private void DeleteNavigationMesh()
        {
            navigationMesh.DeleteNavigationMesh();
        }
    }
}