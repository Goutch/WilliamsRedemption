using System.Collections.Generic;
using UnityEngine;


namespace Light
{
    public class MeshLight : MonoBehaviour
    {
        [Range(0, 360)] [SerializeField] private float coneAngle = 0;
        [Range(0, 360)] [SerializeField] private float faceAngle = 0;
        [SerializeField] [Range(0.1f, 2)] private float precisionInDegree = 1;
        [SerializeField] private float radius = 3;
        [SerializeField] private Color color;

        [SerializeField] private int obstacleLayerIndex = 1;

        [Tooltip("Draw the triangles of the mesh in blue in the scene tab")] [SerializeField]
        private bool debugDraw = false;

        private float startAngle;
        private float endAngle;
        private Mesh mesh;
        private Vector2[] uvs;
        private List<Vector3> vertices;
        private List<int> triangles;

        private void Start()
        {
            mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;
            vertices = new List<Vector3>();
            triangles = new List<int>();
        }

        private void Update()
        {
            //detect playyer
            DetectLightTrigger();
            Scan();
            DrawMesh();
            if (debugDraw) DebugDraw(Color.blue, 0.1f);
            //todo:optomize:
            //todo:update only when a moving obstacle go in it
            //todo:update only when transform move
            //todo:dont make as much triangle just make em at edge of tiles
            //todo:on/off method
            //todo:off when out of camera bound
        }

        private void Scan()
        {
            vertices.Clear();
            triangles.Clear();
            startAngle = coneAngle == 0 ? 0 : faceAngle - coneAngle / 2;
            endAngle = coneAngle == 0 ? 360 : faceAngle + coneAngle / 2;
            Vector2 raycastDirection = new Vector2();
            vertices.Add(new Vector3(0, 0, 0)); // add vertecies 0
            int cornerAIndex = 1;
            int cornerBIndex;
            for (float i = startAngle; i < endAngle; i += precisionInDegree)
            {
                raycastDirection = DegreeToVector(i);
                RaycastHit2D hit =
                    Physics2D.Raycast(this.transform.position, raycastDirection, radius, obstacleLayerIndex, 0);

                if (hit.collider == null)
                {
                    //set ray point to max range
                    hit.point = new Vector2(
                        raycastDirection.x * radius,
                        raycastDirection.y * radius);
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

        private void DrawMesh()
        {
            mesh.Clear();
            mesh.SetVertices(vertices);
            mesh.triangles = triangles.ToArray();
            UpdateUVs();
            UpdateColor();
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

        private void DetectLightTrigger()
        {
            if (LightTrigger.Instance != null)
                if (Vector2.Distance(this.transform.position, LightTrigger.Instance.transform.position) < radius)
                {
                    float AngleRad =
                        Mathf.Atan2(LightTrigger.Instance.transform.position.y - transform.position.y,
                            LightTrigger.Instance.transform.position.x - transform.position.x);
                    //Angle en Degrés
                    float AngleDeg = (180 / Mathf.PI) * AngleRad;
                    if (AngleDeg < 0)
                    {
                        AngleDeg += 360;
                    }

                    if (CheckDegreeWithinCone(AngleDeg))
                    {
                        RaycastHit2D hit =
                            Physics2D.Raycast(transform.position, new Vector2(
                                LightTrigger.Instance.transform.position.x - transform.position.x,
                                LightTrigger.Instance.transform.position.y - transform.position.y), radius);
                        Debug.DrawRay(transform.position, new Vector2(
                            hit.point.x - transform.position.x,
                            hit.point.y - transform.position.y), Color.green);
                        if (hit.collider != null)
                        {
                            if (hit.collider.tag == "Avatar")
                            {
                                print("Inlight");
                                LightTrigger.Instance.NotifyInLight();
                            }

                            print("Hit" + hit.collider.name);
                        }
                    }
                }
        }

        private void AddNewTriangle(int v0, int v1, int v2)
        {
            //add in wrong order to inverse normals
            triangles.Add(v2);
            triangles.Add(v1);
            triangles.Add(v0);
        }

        private bool CheckDegreeWithinCone(float degree)
        {
            if (coneAngle == 0 || coneAngle == 360) return true;

            float min = faceAngle - coneAngle / 2;
            float max = faceAngle + coneAngle / 2;

            if (degree > max)
            {
                return false;
            }

            if (min < 0)
            {
                min = ClampDegree0To360(min);
            }

            return degree > min;
        }

        protected void UpdateUVs()
        {
            uvs = new Vector2[vertices.Count];

            for (int i = 0; i < uvs.Length; i++)
            {
                float u = vertices[i].x / radius / 2 + 0.5f;
                float v = vertices[i].y / radius / 2 + 0.5f;

                u = Clamp(0, u, 1);
                v = Clamp(0, v, 1);

                uvs[i] = new Vector2(u, v);
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

        private float Clamp(float min, float target, float max)
        {
            target = Mathf.Max(min, target);
            target = Mathf.Min(max, target);
            return target;
        }

        public static float ClampDegree0To360(float degree)
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

        public static Vector2 DegreeToVector(float degree)
        {
            float radian = degree * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
        }
    }
}