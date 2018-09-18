using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Harmony
{
    /// <summary>
    /// Inspecteur personalisé pour <see cref="World"/>.
    /// </summary>
    /// <inheritdoc/>
    [CustomEditor(typeof(World), true)]
    public class WorldInspector : BaseInspector
    {
        private World world;

        private ListProperty scenes;
        private BasicProperty activeScene;
        private BasicProperty mainScene;
        private BasicProperty isDeveloppement;

        protected override void Initialize()
        {
            world = target as World;

            scenes = GetListProperty("scenes");
            activeScene = GetBasicProperty("activeScene");
            mainScene = GetBasicProperty("mainScene");
            isDeveloppement = GetBasicProperty("isDeveloppement");
        }

        protected override void Draw()
        {
            if (EditorBuildSettings.scenes.Length == 0)
            {
                DrawErrorBox("You don't have any scene in the Build Settings. You will enconter errors. Generate the Build Settings" +
                             "using \"Tools/Project/Generate Build Settings\".");
            }

            BeginTable();
            BeginTableCol();
            DrawTitleLabel("World tools");

            if (!EditorApplication.isPlaying)
            {
                DrawButton("Open World", OpenWorldInEditor);
            }
            else
            {
                DrawDisabledButton("Open World");
            }

            EndTableCol();
            EndTable();


            DrawTitleLabel("Scenes");
            if (EditorBuildSettings.scenes.Length == 0)
            {
                DrawErrorBox("Builds settings are empty. Please generate them, and make sure the first scene is your \"Main\"" +
                             "scene.");
            }
            else
            {
                var mainSceneIsInWorldScenes = false;
                var mainSceneFileName = Path.GetFileName(EditorBuildSettings.scenes[0].path);
                foreach (R.E.Scene scene in world.Scenes)
                {
                    var sceneFileName = Path.GetFileName(AssetsExtensions.FindScenePath(R.S.Scene.ToString(scene)));
                    if (sceneFileName == mainSceneFileName)
                    {
                        mainSceneIsInWorldScenes = true;
                        break;
                    }
                }

                if (mainSceneIsInWorldScenes)
                {
                    DrawErrorBox("Worlds must not contain the \"Main\" scene (i.e the first scene in the \"Build Settings\")." +
                                 "You will enconter errors.");
                }
            }

            DrawInfoBox("Is a Scene missing ? Go to \"Tools/Project/Generate Const Class\" to generate missing constants.");
            DrawProperty(scenes);

            DrawTitleLabel("Active Scene");
            if (world.ActiveScene != R.E.Scene.None)
            {
                var activeSceneIsInWorldScenes = false;
                foreach (R.E.Scene scene in world.Scenes)
                {
                    if (scene == world.ActiveScene)
                    {
                        activeSceneIsInWorldScenes = true;
                        break;
                    }
                }

                if (!activeSceneIsInWorldScenes)
                {
                    DrawErrorBox("The Active Scene must be part of the world scenes. You will enconter errors.");
                }
            }

            DrawProperty(activeScene);

            DrawTitleLabel("Is Scene used for developpement only ?");
            DrawInfoBox("Worlds marked as \"Developppement only\" will not be included in a release build.");
            DrawPropertyWithLabel(isDeveloppement);

            DrawTitleLabel("Main Scene for developpement");
            DrawWarningBox("In Editor Only for debuging purposes. In a release build, the main scene is " +
                           "the first scene in the build settings.");
            DrawProperty(mainScene);
        }

        private void OpenWorldInEditor()
        {
            //Find startup scene
            string scenePath;
            if (world.MainScene == R.E.Scene.None)
            {
                if (EditorBuildSettings.scenes.Length > 0)
                    scenePath = EditorBuildSettings.scenes[0].path;
                else
                    scenePath = null;
            }
            else
            {
                scenePath = AssetsExtensions.FindScenePath(R.S.Scene.ToString(world.MainScene));
            }

            if (scenePath == null)
            {
                Debug.LogError("Cannot open a world without a startup scene. The startup scene is either the first scene in the" +
                               "Build Settings or the one specified in that world.");
            }
            else
            {
                //Load starting scene. This scene is allways loaded.
                SceneManager.SetActiveScene(EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single));

                //Load World scenes
                foreach (R.E.Scene scene in world.Scenes)
                {
                    var sceneName = R.S.Scene.ToString(scene);
                    scenePath = AssetsExtensions.FindScenePath(sceneName);
                    if (scenePath != null)
                        EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
                    else
                        Debug.LogError("The Scene named \"" + sceneName + "\" of the world you are trying to open seems to have " +
                                       "been renamed or doesn't exist anymore. To solve this problem, re-generate const classes " +
                                       "with the \"Tools/Project/Generate Const Class\" menu and re-assign the Scene.");
                }
            }
        }
    }
}