using Game.Puzzle.Light;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Controller
{
    public class LightController : MonoBehaviour
    {
        private List<MeshLight> meshLights;

        private void Awake()
        {
            meshLights = new List<MeshLight>();

            GameObject[] lights = GameObject.FindGameObjectsWithTag(Values.Tags.Light);
            for (int i = 0; i < lights.Length; ++i)
            {
                GameObject gameObject = lights[i];
                MeshLight meshLight = gameObject.GetComponent<MeshLight>();
                meshLights.Add(meshLight);
            }

        }

        public void UpdateLight()
        {
            foreach (MeshLight light in meshLights)
            {
                light.DrawMesh();
            }
        }

        public void UpdateLight(Vector2 position)
        {
            List<MeshLight> lightsToUpdate = meshLights.FindAll(it => it.gameObject.GetComponent<Collider2D>().bounds.Contains(position));
            foreach (MeshLight light in lightsToUpdate)
            {
                light.DrawMesh();
            }
        }

        public void UpdateLightAtEndOfFrame(Vector2 position)
        {
            StartCoroutine(UpdateLightCoroutine(position));
        }

        private IEnumerator UpdateLightCoroutine(Vector2 position)
        {
            yield return new WaitForEndOfFrame();
            UpdateLight(position);
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