using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Générateur de "Build Settings". Essentiellement, génère la liste des scènes à partir des Worlds présents dans le projet.
    /// </summary>
    public static class BuildSettingsGenerator
    {
        private const string MainSceneName = "Main";

        /// <summary>
        /// Démarre la génération des "Build Settings" et ouvre la fenêtre des "Build Settings".
        /// </summary>
        /// <seealso cref="GenerateBuildSettings"/>
        [MenuItem("Tools/Project/Generate Build Settings", priority = 101)]
        public static void GenerateBuildSettingsAndOpen()
        {
            GenerateBuildSettings();
            OpenBuildSettingsWindow();
        }

        /// <summary>
        /// Démarre la génération des "Build Settings".
        /// </summary>
        /// <seealso cref="GenerateBuildSettingsAndOpen"/>
        public static void GenerateBuildSettings()
        {
            var scenes = new List<EditorBuildSettingsScene>();
            foreach (var scenePath in GetWorldsScenesPaths())
            {
                scenes.Add(new EditorBuildSettingsScene(scenePath, true));
            }
            EditorBuildSettings.scenes = scenes.ToArray();
        }

        private static void OpenBuildSettingsWindow()
        {
            EditorWindow.GetWindow(Type.GetType("UnityEditor.BuildPlayerWindow,UnityEditor"));
        }

        private static IEnumerable<string> GetWorldsScenesPaths()
        {
            var scenePaths = new HashSet<string>();

            //Main scene
            var mainScenePath = AssetsExtensions.FindScenePath(MainSceneName);
            if (mainScenePath != null)
                scenePaths.Add(mainScenePath);
            else
                Debug.LogError("No main scene found. Please create a scene named \"" + MainSceneName + "\", re-generate const classes " +
                               "and try again.");

            //World scenes
            foreach (var world in AssetsExtensions.FindAssets<World>())
            {
                if (!world.IsDeveloppement)
                {
                    foreach (var scene in world.Scenes)
                    {
                        var sceneName = R.S.Scene.ToString(scene);
                        var scenePath = AssetsExtensions.FindScenePath(sceneName);

                        if (scenePath != null)
                            scenePaths.Add(scenePath);
                        else
                            Debug.LogError("The scene named \"" + sceneName + "\" found in World named \"" + world.name + "\" seems to have been " +
                                           "renamed or doesn't exist anymore. Remove it from this world or try to re-generate const classes and try " +
                                           "again.");
                    }
                }
            }

            return scenePaths;
        }
    }
}