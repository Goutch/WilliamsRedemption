using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Light
{
    [ExecuteInEditMode]
    public abstract class MeshLight : MonoBehaviour
    {
        [SerializeField] protected Color color = Color.white;
        [SerializeField] protected int obstacleLayerIndex = 1;

        [Tooltip("Draw the triangles of the mesh in blue in the scene tab")] [SerializeField]
        protected bool debugDraw = false;

        [SerializeField] private bool updateEveryFrame = false;
        [SerializeField] private float movingObstacleUpdateStopCooldown = 1;
        [SerializeField] private bool hasMovingObstaclesInRange = false;
        private bool isOpen=true;

        public bool HasMovingObstaclesInRange
        {
            get { return hasMovingObstaclesInRange; }
            set
            {
                if (value)
                    hasMovingObstaclesInRange = value;
                else
                    StartCoroutine(StartUpdateCooldownRoutine(movingObstacleUpdateStopCooldown));
            }
        }

        protected Mesh mesh;
        protected Vector2[] uvs;
        protected List<Vector3> vertices;
        protected List<int> triangles;

        public abstract LightSensor IsWithinLightLimits(Vector2 position);
        protected abstract void Scan();
        protected abstract void UpdateUVs();

        protected void Start()
        {
            InitializeComponent();
            this.gameObject.layer = 2;
            mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;
            vertices = new List<Vector3>();
            triangles = new List<int>();
        }

        protected void DrawMesh()
        {
            Scan();
            mesh.Clear();
            mesh.SetVertices(vertices);
            mesh.triangles = triangles.ToArray();
            UpdateUVs();
            UpdateColor();
            if (debugDraw)
                DebugDraw(Color.red, 0.1f);
        }

        protected void DebugDraw(Color color, float time)
        {
            for (int i = 0; i < triangles.Count; i++)
            {
                Debug.DrawLine(vertices[triangles[i]] + transform.position,
                    vertices[triangles[++i]] + transform.position,
                    color, time);
                Debug.DrawLine(vertices[triangles[i]] + transform.position,
                    vertices[triangles[++i]] + transform.position,
                    color, time);
                Debug.DrawLine(vertices[triangles[i]] + transform.position,
                    vertices[triangles[i - 2]] + transform.position,
                    color, time);
            }
        }

        protected void Update()
        {
            if (isOpen)
            {
                if (updateEveryFrame || HasMovingObstaclesInRange)
                {
                    DrawMesh();
                }
            }
        }

        protected void AddNewTriangle(int v0, int v1, int v2)
        {
            //add in wrong order to inverse normals
            triangles.Add(v2);
            triangles.Add(v1);
            triangles.Add(v0);
        }

        protected virtual void InitializeComponent()
        {
            if (!GetComponent<MeshRenderer>())
            {
                this.gameObject.AddComponent<MeshRenderer>();
            }

            if (!GetComponent<MeshFilter>())
            {
                this.gameObject.AddComponent<MeshFilter>();
            }

            if (!GetComponent<LightStimulu>())
            {
                this.gameObject.AddComponent<LightStimulu>();
            }
        }

        protected Vector2 DegreeToVector(float degree)
        {
            float radian = degree * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
        }

        public float ClampDegree0To360(float degree)
        {
            while (degree >= 360)
            {
                degree -= 360;
            }

            while (degree < 0)
            {
                degree += 360;
            }

            return degree;
        }

        protected void UpdateColor()
        {
            Color32[] colors32 = new Color32[vertices.Count];
            for (int i = 0; i < colors32.Length; i++)
            {
                colors32[i] = color;
            }

            mesh.colors32 = colors32;
        }

        protected float VectorToDegree(Vector2 dir)
        {
            float AngleRad = Mathf.Atan2(dir.x, dir.y);
            float AngleDeg = (180 / Mathf.PI) * AngleRad;

            return ClampDegree0To360(AngleDeg + 90);
        }

        protected IEnumerator StartUpdateCooldownRoutine(float cooldown)
        {
            yield return new WaitForSeconds(cooldown);
            hasMovingObstaclesInRange = false;
        }

        private void Open()
        {
            isOpen = true;
            DrawMesh();
        }

        private void Close()
        {
            isOpen = false;
            mesh.Clear();
            vertices.Clear();
        }
    }
}