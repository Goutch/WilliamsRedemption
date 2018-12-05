using System.Diagnostics.Contracts;
using UnityEngine;

namespace Game.Controller
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform followPoint;
        [SerializeField] private float positionZ;
        private Vector3 initialBackgroundScale;
        private Transform background;
        private bool follow = true;
        private float initialSize;

        public bool Follow
        {
            get { return follow; }

            set { follow = value; }
        }

        private void Awake()
        {
            initialSize = Camera.main.orthographicSize;
            background = GetComponentInChildren<Transform>();
            initialBackgroundScale = background.localScale;
        }

        private void Update()
        {
            if (follow && followPoint != null)
            {
                Camera.main.transform.position = new Vector3(followPoint.position.x, followPoint.position.y, positionZ);
            }
        }

        public void FixPoint(Vector2 point, float size)
        {
            Camera.main.transform.position = new Vector3(point.x, point.y, positionZ);
            Camera.main.orthographicSize = size;
            float changeRatio = Camera.main.orthographicSize / initialSize;

            background.localScale *= changeRatio;
            
            follow = false;
        }

        public void ResumeFollow()
        {
            follow = true;
            background.localScale =initialBackgroundScale;
            Camera.main.orthographicSize = initialSize;
        }
    }
}