using UnityEditor;

namespace Harmony
{
    /// <summary>
    /// Inspecteur personalisé pour <see cref="PathFinder"/>.
    /// </summary>
    /// <inheritdoc/>
    [CustomEditor(typeof(PathFinder), true)]
    public class PathFinderInspector : BaseInspector
    {
        private PathFinder pathFinder;
        
        protected override void Initialize()
        {
            pathFinder = target as PathFinder;
        }

        protected override void Draw()
        {
            Initialize();
            DrawDefault();
            
            BeginTable();
            BeginTableCol();
            DrawTitleLabel("Path Finder utils");

            if (!GetBasicProperty("debugPathStartPoint").IsNull() && !GetBasicProperty("debugPathEndPoint").IsNull())
            {
                DrawButton("Compute Debug Path", ComputeAndShowDebugPath);
                DrawButton("Clear Debug Path", ClearDebugPath);
            }
            else
            {
                DrawInfoBox("You must set the start point and end point before you can test the pathfinding method.");
            }

            EndTableCol();
            EndTable();
        }

        private void OnDestroy()
        {
            ClearDebugPath();
        }

        private void ComputeAndShowDebugPath()
        {
            pathFinder.ComputeDebugPath();
        }

        private void ClearDebugPath()
        {
            pathFinder.ClearDebugPath();
        }
    }
}