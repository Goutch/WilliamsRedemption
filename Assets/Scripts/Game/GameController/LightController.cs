using Game.Puzzle.Light;
using System.Collections;
using UnityEngine;

namespace Game.Controller
{
    public class LightController : MonoBehaviour
    {
        MeshLight[] meshLights;

        private void Awake()
        {
            GameObject[] lights = GameObject.FindGameObjectsWithTag(Values.Tags.Light);
            meshLights = new MeshLight[lights.Length];
            for (int i = 0; i < lights.Length; ++i)
                meshLights[i] = (lights.GetValue(i) as GameObject).GetComponent<MeshLight>();
        }

        public void UpdateLight()
        {
            foreach (MeshLight light in meshLights)
            {
                light.DrawMesh();
            }
        }


        public void UpdateLightAtEndOfFrame()
        {
            StartCoroutine(UpdateLightCoroutine());
        }

        private IEnumerator UpdateLightCoroutine()
        {
            yield return new WaitForEndOfFrame();
            UpdateLight();
        }
    }
}