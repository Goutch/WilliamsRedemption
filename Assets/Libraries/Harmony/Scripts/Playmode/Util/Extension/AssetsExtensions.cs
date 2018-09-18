using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Harmony
{
#if UNITY_EDITOR
    /// <summary>
    /// Contient nombre de méthodes d'extensions pour faciliter la manipulation des Assets de l'éditeur.
    /// </summary>
    public static class AssetsExtensions
    {
        /// <summary>
        /// Permet d'obtenir l'Asset avec le GUID donné.
        /// </summary>
        /// <typeparam name="T">Type de l'Asset à obtenir.</typeparam>
        /// <param name="guid">GUID de l'Asset à obtenir.</param>
        /// <returns>Asset avec le GUID donné, si trouvé. Sinon, retourne null.</returns>
        public static T FindAsset<T>(string guid) where T : Object
        {
            var assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
            if (assetPath != null)
            {
                return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(assetPath);
            }
            return null;
        }

        /// <summary>
        /// Permet d'obtenir tous les Assets d'un type donné.
        /// </summary>
        /// <typeparam name="T">Type de l'Asset à obtenir.</typeparam>
        /// <returns>Liste de tous les Assets du type donné dans le projet.</returns>
        public static IList<T> FindAssets<T>() where T : Object
        {
            var assets = new List<T>();
            foreach (var assetGuid in UnityEditor.AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T))))
            {
                var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(UnityEditor.AssetDatabase.GUIDToAssetPath(assetGuid));
                if (asset != null)
                {
                    assets.Add(asset);
                }
            }
            return assets;
        }

        /// <summary>
        /// Permet d'obtenir le chemin vers une Scene avec le nom donné.
        /// </summary>
        /// <param name="name">Nom de la scene à obtenir.</param>
        /// <returns>Chemin vers la scene avec le nom donné.</returns>
        public static string FindScenePath(string name)
        {
            var scenesPath = Directory.GetFiles(Path.GetFullPath(Application.dataPath), name + ".unity", SearchOption.AllDirectories);

            if (scenesPath.Length == 1)
            {
                return scenesPath[0].Replace(Path.GetFullPath(Path.Combine(Application.dataPath, "..")) + Path.DirectorySeparatorChar, "");
            }

            return null;
        }

        /// <summary>
        /// Permet d'obtenir le chemin vers toutes les Scene dans un dossier.
        /// </summary>
        /// <param name="folderPath">Dossier à recherche de manière récursive.</param>
        /// <returns>Chemins vers les scene dans le dossier donné.</returns>
        public static IList<string> FindScenesPathIn(string folderPath)
        {
            var scenesPath = Directory.GetFiles(Path.GetFullPath(Path.Combine(Application.dataPath, folderPath)),
                                                     "*.unity",
                                                     SearchOption.AllDirectories);

            for (var i = 0; i < scenesPath.Length; i++)
            {
                scenesPath[i] = scenesPath[i].Replace(Path.GetFullPath(Path.Combine(Application.dataPath, "..")) + "\\", "");
            }

            return scenesPath;
        }
    }
#endif
}
