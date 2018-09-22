using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Light
{
    public class RectangleLight : MeshLight
    {
        [SerializeField] private float width=30;
        [SerializeField] private float range=20;
        [Range(0.01f, 1)] [SerializeField] private float precisionInTile = 0.1f;
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
            base.Start();
            Width = width;
            Range = range;
            DrawMesh();
        }


        protected override void Scan()
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

        protected override void UpdateUVs()
        {
            Vector2[] uvs = new Vector2[vertices.Count];

            for (int i = 0; i < uvs.Length; i++)
            {
                //uvs[i] = new Vector2(vertices[i].x, vertices[i].y);
                uvs[i] = new Vector2((vertices[i].x / width), (vertices[i].y / range));
            }

            mesh.uv = uvs;
        }

        protected override void UpdateColor()

        {
            Color32[] colors32 = new Color32[vertices.Count];
            for (int i = 0; i < colors32.Length; i++)
            {
                colors32[i] = color;
            }

            mesh.colors32 = colors32;
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