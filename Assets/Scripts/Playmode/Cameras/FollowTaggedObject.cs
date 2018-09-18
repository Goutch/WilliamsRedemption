using Harmony;
using UnityEngine;

namespace Playmode.Cameras
{
    /// <summary>
    /// Camera that follows a specified target.
    /// </summary>
    /// <remarks>
    /// Updates target only on Start and on World load.
    /// </remarks>
    [AddComponentMenu("Game/Cameras/FollowTaggedObject")]
    public class FollowTaggedObject : MonoBehaviour
    {
        [SerializeField] private R.E.Tag targetTag = R.E.Tag.None;

        private Transform rootTransform;
        private WorldLoader worldLoader;
        private Transform targetTransform;

        private void Awake()
        {
            rootTransform = this.Root().transform;
            worldLoader = this.GetComponentInTaggedObject<WorldLoader>(R.S.Tag.GlobalComponents);
        }

        private void OnEnable()
        {
            worldLoader.OnWorldLoadingEnded += RefreshTarget;
        }

        private void Start()
        {
            RefreshTarget();
        }

        private void RefreshTarget()
        {
            var targetGameObject = GameObject.FindWithTag(R.S.Tag.ToString(targetTag));
            targetTransform = targetGameObject != null ? targetGameObject.transform : null;
        }

        private void Update()
        {
            if (targetTransform != null)
            {
                //Keep the Z axis position of the camera. Otherwise, the target object will be considered behind the camera.
                rootTransform.position = new Vector3(targetTransform.position.x,
                                                     targetTransform.position.y,
                                                     rootTransform.position.z);
            }
        }

        private void OnDisable()
        {
            worldLoader.OnWorldLoadingEnded -= RefreshTarget;
        }
    }
}