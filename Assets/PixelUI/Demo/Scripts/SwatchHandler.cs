/******************************************************************************************
 * Name: SwatchHandler.cs
 * Created by: Jeremy Voss
 * Created on: 9/12/2018
 * Last Modified: 9/12/2018
 * Owned by: Pixelsoft Games, LLC.
 * Description:
 * A simple demo script used to demonstrate how Pixel UI is set up to change color easily
 * via a material.  It also demonstrates how the UI appears in various colors.
 ******************************************************************************************/
using UnityEngine;
using UnityEngine.UI;

namespace PixelsoftGames.PixelUI
{
    public class SwatchHandler : MonoBehaviour
    {
        #region Private Serialized Variables

        /// <summary>
        /// The Pixel UI material that can be used to easily change the entire interface coloring
        /// </summary>
        [Tooltip("The pixel UI material we will use to demonstrate color changing")]
        [SerializeField]
        Material pixelUIMaterial;

        #endregion

        #region Public Methods

        /// <summary>
        /// We take the color of the passed image and use that color to change our material color
        /// for demonstration.
        /// </summary>
        public void ChangeColor(Image image)
        {
            if (image == null || pixelUIMaterial == null)
                return;

            pixelUIMaterial.color = image.color;
        }

        #endregion

        #region MonoBehaviour Callbacks

        /// <summary>
        /// If we do not change the default color of the material back to white then when we stop our play tests
        /// it will not revert to it's normal color and stay the color we set it to.
        /// </summary>
        private void OnDestroy()
        {
            if (pixelUIMaterial != null)
                pixelUIMaterial.color = Color.white;
        }

        #endregion
    }
}