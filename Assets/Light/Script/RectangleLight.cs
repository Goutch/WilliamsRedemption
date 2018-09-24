using Harmony;
using UnityEngine;

namespace Light
{
    public class RectangleLight : MeshLight
    {
        [SerializeField] private float width = 30;
        [SerializeField] private float range = 20;
        [Range(0.01f, 1)] [SerializeField] private float raycastPrecision = 0.1f;

        public float Width
        {
            get { return width; }
            set
            {
                width = value;
                GetComponent<LightStimulu>().OnDimentionsChange(new Vector2(width, range));
            }
        }

        public float Range
        {
            get { return range; }
            set
            {
                range = value;
                GetComponent<LightStimulu>().OnDimentionsChange(new Vector2(width, range));
            }
        }

        private new void Start()
        {
            base.Start();
            Width = width;
            Range = range;
            DrawMesh();
        }


        protected override void Scan()
        {
            vertices.Clear();
            triangles.Clear();

            Vector2 raycastDirection = Vector2.down;

            for (float i = -Width / 2; i <= Width / 2; i += raycastPrecision)
            {
                Vector2 raycastBeginPos = new Vector2(i, 0);
                Vector2 raycastEndPos;

                RaycastHit2D hit =
                    Physics2D.Raycast(raycastBeginPos + (Vector2) transform.position,
                        raycastDirection, range,
                        obstacleLayerIndex, 0);

                if (hit.collider == null)
                {
                    hit.point = new Vector2(
                        (raycastDirection.x * range) + i,
                        raycastDirection.y * range);
                }
                else
                {
                    hit.point = hit.point - (Vector2) transform.position;
                }

                raycastEndPos = hit.point;

                vertices.Add(raycastBeginPos);
                vertices.Add(raycastEndPos);
            }

            for (int i = 0; i < vertices.Count - 2; ++i)
            {
                AddNewTriangle(i, i + 1, i + 2);
            }
        }

        protected override void UpdateUVs()
        {
            Vector2[] uvs = new Vector2[vertices.Count];

            for (int i = 0; i < uvs.Length; i++)
            {
                uvs[i] = new Vector2((vertices[i].x / width), (vertices[i].y / range));
            }

            mesh.uv = uvs;
        }

        protected override void InitializeComponent()
        {
            base.InitializeComponent();

            if (!GetComponent<BoxCollider2D>())
            {
                this.gameObject.AddComponent<BoxCollider2D>();
                this.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
            }
        }

        public override LightSensor IsWithinLightLimits(Vector2 position)
        {
            Vector2 startPos = (position * Vector2.right) + (transform.position * Vector2.up);
            RaycastHit2D hit = Physics2D.Raycast(startPos, Vector2.down, range);

            if (hit.collider != null)
            {
                Debug.DrawLine(startPos, hit.point, Color.green);
                return hit.collider.Root().GetComponent<LightSensor>();
            }

            return null;
        }
    }
}