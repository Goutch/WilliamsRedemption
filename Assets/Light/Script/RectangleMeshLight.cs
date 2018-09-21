using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Light
{
    [ExecuteInEditMode]
    public class RectangleMeshLight : MonoBehaviour, MeshLight
    {
        [SerializeField] private float width;
        [SerializeField] private float range;
        [Range(0.01f, 1)] [SerializeField] private float precisionInTile = 1;
        [SerializeField] private Color color = Color.white;
        [SerializeField] private int obstacleLayerIndex = 1;
        [SerializeField] private bool debugDraw = false;
        private Mesh mesh;
        private Vector2[] uvs;
        private List<Vector3> vertices;
        private List<int> triangles;

        public float Width
        {
            get { return width; }
            set
            {
                width = value;
                GetComponent<LightStimulu>().OndDimentionsChange(new Vector2(width, range));
            }
        }

        public float Range
        {
            get { return range; }
            set
            {
                range = value;
                GetComponent<LightStimulu>().OndDimentionsChange(new Vector2(width, range));
            }
        }

        private void Start()
        {
            InitializeComponent();
            Width = width;
            Range = range;
            this.gameObject.layer = 2;
            InitializeComponent();
            mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;
            vertices = new List<Vector3>();
            triangles = new List<int>();
        }

        private void Update()
        {
            Scan();
            DrawMesh();
            if (debugDraw)
                DebugDraw(Color.red, 1);
        }

        private void Scan()
        {
            vertices.Clear();
            triangles.Clear();
            Vector2 RaycastBeginPos;
            Vector2 RaycastEndPos;
            Vector2 raycastDirection = Vector2.down;
            // add vertecies 0
            for (float i = -Width / 2; i <= Width / 2; i += precisionInTile)
            {
                RaycastBeginPos = new Vector2(i, 0);

                RaycastHit2D hit =
                    Physics2D.Raycast(RaycastBeginPos + (Vector2) transform.position,
                        raycastDirection, range,
                        obstacleLayerIndex, 0);

                //ray hit nothing
                if (hit.collider == null)
                {
                    //set hitA hit point to max range
                    hit.point = new Vector2(
                        (raycastDirection.x * range) + i,
                        raycastDirection.y * range);
                }
                //ray hit something
                else
                {
                    hit.point = hit.point - (Vector2) transform.position;
                }

                RaycastEndPos = hit.point;
                vertices.Add(RaycastBeginPos);
                vertices.Add(RaycastEndPos);
            }

            for (int i = 2; i < vertices.Count; i++)
            {
                AddNewTriangle(i - 2, i - 1, i);
            }
        }

        private void AddNewTriangle(int v0, int v1, int v2)
        {
            //add in wrong order to inverse normals
            triangles.Add(v2);
            triangles.Add(v1);
            triangles.Add(v0);
        }

        private void DrawMesh()
        {
            mesh.Clear();
            mesh.SetVertices(vertices);
            mesh.triangles = triangles.ToArray();
            UpdateUVs();
            UpdateColor();
        }

        protected void UpdateUVs()
        {
            Vector2[] uvs = new Vector2[vertices.Count];

            for (int i = 0; i < uvs.Length; i++)
            {
                //uvs[i] = new Vector2(vertices[i].x, vertices[i].y);
                uvs[i] = new Vector2((vertices[i].x / width), (vertices[i].y / range));
            }

            mesh.uv = uvs;
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

        private void InitializeComponent()
        {
            if (!GetComponent<MeshRenderer>())
            {
                this.gameObject.AddComponent<MeshRenderer>();
            }

            if (!GetComponent<MeshFilter>())
            {
                this.gameObject.AddComponent<MeshFilter>();
            }

            if (!GetComponent<BoxCollider2D>())
            {
                this.gameObject.AddComponent<BoxCollider2D>();
                this.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
            }

            if (!GetComponent<LightStimulu>())
            {
                this.gameObject.AddComponent<LightStimulu>();
            }
        }

        private void DebugDraw(Color color, float time)
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

        public LightSensor IsWithinLightLimits(Vector2 position)
        {
            Vector2 startPos = (position * Vector2.right) + (transform.position * Vector2.up);
            RaycastHit2D hit = Physics2D.Raycast(startPos, Vector2.down, range, obstacleLayerIndex, 0);

            if (hit.collider != null)
            {
                Debug.DrawLine(startPos + (transform.position * Vector2.up), hit.point, Color.green);
                print("Hit" + hit.collider.name);
                return hit.collider.GetComponent<LightSensor>();
            }

            return null;
        }
    }
}