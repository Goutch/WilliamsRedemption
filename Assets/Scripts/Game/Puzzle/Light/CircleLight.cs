using Harmony;
using UnityEngine;

namespace Game.Puzzle.Light
{
    [ExecuteInEditMode]
    public class CircleLight : MeshLight
    {
        [Range(0, 360)] [SerializeField] private float coneAngle;
        [Range(0, 360)] [SerializeField] private float faceAngle;
        [SerializeField] private float radius = 3;
        [SerializeField] [Range(0.1f, 2)] private float precisionInDegree = 1;

        private float startAngle;
        private float endAngle;

        private Vector3 centerVertices = Vector3.zero;

        public float Radius
        {
            get { return radius; }
            set
            {
                radius = value;
                GetComponent<LightStimulu>().OnRadiusChange(radius);
            }
        }

        public float FaceAngle
        {
            get { return faceAngle; }
            set { faceAngle = value; }
        }

        private new void Start()
        {
            base.Start();
            Radius = radius;
            DrawMesh();
        }

        protected override void InitializeComponent()
        {
            base.InitializeComponent();
            if (!GetComponent<CircleCollider2D>())
            {
                this.gameObject.AddComponent<CircleCollider2D>();
                this.gameObject.GetComponent<CircleCollider2D>().isTrigger = true;
            }
        }


        protected override void Scan()
        {
            Vector2 raycastDirection = new Vector2();

            vertices.Clear();
            triangles.Clear();

            startAngle = coneAngle == 0 ? 0 : faceAngle - coneAngle / 2;
            endAngle = coneAngle == 0 ? 360 : faceAngle + coneAngle / 2;

            vertices.Add(centerVertices);

            int cornerAIndex = 1;
            int cornerBIndex;
            for (float i = startAngle; i < endAngle; i += precisionInDegree)
            {
                raycastDirection = DegreeToVector(i);
                RaycastHit2D hit =
                    Physics2D.Raycast(this.transform.position, raycastDirection, Radius, obstacleLayer, 0);

                if (hit.collider == null)
                {
                    hit.point = new Vector2(
                        raycastDirection.x * Radius,
                        raycastDirection.y * Radius);
                }
                else
                {
                    hit.point = hit.point - (Vector2) transform.position;
                }

                vertices.Add(hit.point);
                cornerBIndex = vertices.Count - 1;
                AddNewTriangle(0, cornerAIndex, cornerBIndex);
                cornerAIndex = cornerBIndex;
            }

            if (coneAngle == 360 || coneAngle == 0)
            {
                AddNewTriangle(0, 1, vertices.Count - 1);
            }
        }

        private bool CheckDegreeWithinCone(float degree)
        {
            if (coneAngle == 0 || coneAngle == 360) return true;

            float min = faceAngle - coneAngle / 2;
            float max = faceAngle + coneAngle / 2;

//            if (degree > max)
//            {
//                return false;
//            }
//
//            if (min < 0)
//            {
//                min = ClampDegree0To360(min);
//            }

            return (degree >= min && degree <= max); //ajout de la deuxieme condition et les =
        }

        protected override void UpdateUVs()
        {
            uvs = new Vector2[vertices.Count];

            for (int i = 0; i < uvs.Length; ++i)
            {
                float u = vertices[i].x / Radius / 2 + 0.5f;
                float v = vertices[i].y / Radius / 2 + 0.5f;

                u = Clamp(0, u, 1);
                v = Clamp(0, v, 1);

                uvs[i] = new Vector2(u, v);
            }

            mesh.uv = uvs;
        }

        private float Clamp(float min, float target, float max)
        {
            target = Mathf.Max(min, target);
            target = Mathf.Min(max, target);
            return target;
        }

        public override LightSensor IsWithinLightLimits(Vector2 position)
        {
            if (isOpen)
            {
                float AngleDeg = VectorToDegree(position - (Vector2) transform.position);

                if (CheckDegreeWithinCone(AngleDeg))
                {
                    RaycastHit2D hit =
                        Physics2D.Raycast(transform.position, position - (Vector2) transform.position, Radius,
                            detectionLayers);

                    Debug.DrawRay(transform.position, new Vector2(
                        hit.point.x - transform.position.x,
                        hit.point.y - transform.position.y), Color.green);

                    if (hit.collider != null)
                    {
                        return hit.collider.Root().GetComponent<LightSensor>();
                    }
                }
            }

            return null;
        }
    }
}