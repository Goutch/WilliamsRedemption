
using UnityEngine;
using UnityEditor;

namespace PixelsoftGames.PixelUI
{
    public class PixelUIMenu : MonoBehaviour
    {
        #region Private Constants

        const string roundedPath = "Prefabs/Rounded/";
        const string squarePath = "Prefabs/Square/";
        const string path = "Prefabs/";

        #endregion

        #region Rounded Menu Items

        [MenuItem("Pixel UI/Create Rounded/Button")]
        static void CreateRoundedButton()
        {
            InstantiateObj(roundedPath + "Pixel Button");
        }

        [MenuItem("Pixel UI/Create Rounded/Shadow Button")]
        static void CreateRoundedShadowButton()
        {
            InstantiateObj(roundedPath + "Pixel Button (Shadow)");
        }

        [MenuItem("Pixel UI/Create Rounded/Dropdown")]
        static void CreateRoundedDropdown()
        {
            InstantiateObj(roundedPath + "Pixel Dropdown");
        }

        [MenuItem("Pixel UI/Create Rounded/Shadow Dropdown")]
        static void CreateRoundedShadowDropdown()
        {
            InstantiateObj(roundedPath + "Pixel Dropdown (Shadow)");
        }

        [MenuItem("Pixel UI/Create Rounded/Input")]
        static void CreateRoundedInput()
        {
            InstantiateObj(roundedPath + "Pixel Input");
        }

        [MenuItem("Pixel UI/Create Rounded/Shadow Input")]
        static void CreateRoundedShadowInput()
        {
            InstantiateObj(roundedPath + "Pixel Input (Shadow)");
        }

        [MenuItem("Pixel UI/Create Rounded/Panel")]
        static void CreateRoundedPanel()
        {
            InstantiateObj(roundedPath + "Pixel Panel");
        }

        [MenuItem("Pixel UI/Create Rounded/Radio Button")]
        static void CreateRoundedRadioButton()
        {
            InstantiateObj(roundedPath + "Pixel Radio Button");
        }

        [MenuItem("Pixel UI/Create Rounded/Scrollbar")]
        static void CreateRoundedScollbar()
        {
            InstantiateObj(roundedPath + "Pixel Scrollbar");
        }

        [MenuItem("Pixel UI/Create Rounded/Shadow Scrollbar")]
        static void CreateRoundedShadowScrollbar()
        {
            InstantiateObj(roundedPath + "Pixel Scrollbar (Shadow)");
        }

        [MenuItem("Pixel UI/Create Rounded/Slider")]
        static void CreateRoundedSlider()
        {
            InstantiateObj(roundedPath + "Pixel Slider");
        }

        [MenuItem("Pixel UI/Create Rounded/Toggle (Checkmark)")]
        static void CreateRoundedToggleCheckmark()
        {
            InstantiateObj(roundedPath + "Pixel Toggle (Checkmark)");
        }

        [MenuItem("Pixel UI/Create Rounded/Toggle (Cross)")]
        static void CreateRoundedToggleCross()
        {
            InstantiateObj(roundedPath + "Pixel Toggle (Cross)");
        }

        #endregion

        #region Square Menu Items

        [MenuItem("Pixel UI/Create Square/Button")]
        static void CreateSquareButton()
        {
            InstantiateObj(squarePath + "Pixel Button");
        }

        [MenuItem("Pixel UI/Create Square/Shadow Button")]
        static void CreateSquareShadowButton()
        {
            InstantiateObj(squarePath + "Pixel Button (Shadow)");
        }

        [MenuItem("Pixel UI/Create Square/Dropdown")]
        static void CreateSquareDropdown()
        {
            InstantiateObj(squarePath + "Pixel Dropdown");
        }

        [MenuItem("Pixel UI/Create Square/Dropdown (Shadow)")]
        static void CreateSquareShadowDropdown()
        {
            InstantiateObj(squarePath + "Pixel Dropdown (Shadow)");
        }

        [MenuItem("Pixel UI/Create Square/Input")]
        static void CreateSquareInput()
        {
            InstantiateObj(squarePath + "Pixel Input");
        }

        [MenuItem("Pixel UI/Create Square/Shadow Input")]
        static void CreateSquareShadowInput()
        {
            InstantiateObj(squarePath + "Pixel Input (Shadow)");
        }

        [MenuItem("Pixel UI/Create Square/Panel")]
        static void CreateSquarePanel()
        {
            InstantiateObj(squarePath + "Pixel Panel");
        }

        [MenuItem("Pixel UI/Create Square/Radio Button")]
        static void CreateSquareRadioButton()
        {
            InstantiateObj(squarePath + "Pixel Radio Button");
        }

        [MenuItem("Pixel UI/Create Square/Scrollbar")]
        static void CreateSquareScollbar()
        {
            InstantiateObj(squarePath + "Pixel Scrollbar");
        }

        [MenuItem("Pixel UI/Create Square/Shadow Scrollbar")]
        static void CreateSquareShadowScrollbar()
        {
            InstantiateObj(squarePath + "Pixel Scrollbar (Shadow)");
        }

        [MenuItem("Pixel UI/Create Square/Slider")]
        static void CreateSquareSlider()
        {
            InstantiateObj(squarePath + "Pixel Slider");
        }

        [MenuItem("Pixel UI/Create Square/Toggle (Checkmark)")]
        static void CreateSquareToggleCheckmark()
        {
            InstantiateObj(squarePath + "Pixel Toggle (Checkmark)");
        }

        [MenuItem("Pixel UI/Create Square/Toggle (Cross)")]
        static void CreateSquareToggleCross()
        {
            InstantiateObj(squarePath + "Pixel Toggle (Cross)");
        }

        #endregion

        #region Miscellaneous Menu Items

        [MenuItem("Pixel UI/Create Text")]
        static void CreateText()
        {
            InstantiateObj(path + "Pixel Text");
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// Retrieves prefabs from resources and instantiates on a canvas.
        /// </summary>
        static void InstantiateObj(string fullPath)
        {
            var prefab = Resources.Load(fullPath);
            var canvas = FindObjectOfType<Canvas>();

            if (canvas != null)
                Instantiate(prefab, canvas.transform, false);
            else
                Instantiate(prefab, Vector3.zero, Quaternion.identity);
        }

        #endregion
    }
}