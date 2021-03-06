﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Puzzle.Light
{
    [ExecuteInEditMode]
    public abstract class MeshLight : MonoBehaviour, ITriggerable
    {
        [SerializeField] protected Color color = Color.white;
        [SerializeField] protected LayerMask obstacleLayer;

        [Tooltip("Draw the triangles of the mesh in blue in the scene tab")] [SerializeField]
        protected bool debugDraw = false;

        [SerializeField] private bool updateEveryFrame = false;
        [SerializeField] private float movingObstacleUpdateStopCooldown = 1;
        [SerializeField] private bool hasMovingObstaclesInRange = false;
        [SerializeField] protected bool isOpen;
        [SerializeField] protected LayerMask detectionLayers;
        [SerializeField] protected Sprite closeSprite;
        [SerializeField] private ParticleSystem particleSystem;
        [SerializeField] private ParticleSystem closeSystem;
        [SerializeField] private Sprite openSprite;

        private bool isLocked;
        private bool permanentlyLocked;
        private MeshRenderer renderer;
        private SpriteRenderer spriteRenderer;

        

        public bool UpdateEveryFrame
        {
            get { return updateEveryFrame; }
            private set { updateEveryFrame = value; }
        }

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

        private void Awake()
        {
            vertices = new List<Vector3>();
            triangles = new List<int>();
            mesh = new Mesh();
            permanentlyLocked = false;

            spriteRenderer = GetComponentInParent<SpriteRenderer>();
        }

        protected void Start()
        {
            InitializeComponent();
            this.gameObject.layer = 2;
            GetComponent<MeshFilter>().mesh = mesh;
            isLocked = false;
            renderer = GetComponent<MeshRenderer>();
            if (isOpen)
                Open();
            else
                Close();
        }

        public void DrawMesh()
        {
            Scan();
            mesh.Clear();
            mesh.SetVertices(vertices);
            mesh.triangles = triangles.ToArray();
            UpdateUVs();
            UpdateColor();
            if (debugDraw)
                DebugDraw(Color.red, 0.1f);
            transform.localRotation = transform.root.localRotation;
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
            float AngleRad = Mathf.Atan2(dir.y, dir.x); // jai inverser Y et X
            float AngleDeg = (180 * AngleRad / Mathf.PI);

            return ClampDegree0To360(AngleDeg); // jai supprimer le +90
        }

        protected IEnumerator StartUpdateCooldownRoutine(float cooldown)
        {
            yield return new WaitForSeconds(cooldown);
            hasMovingObstaclesInRange = false;
        }

        public void Open()
        {
            isOpen = true;
            DrawMesh();
            if (renderer != null)
            {
                renderer.enabled = true;
            }

            if (spriteRenderer != null)
                spriteRenderer.sprite = openSprite;

            if (particleSystem != null)
                particleSystem.gameObject.SetActive(true);

            if (closeSystem != null)
                closeSystem.gameObject.SetActive(false);
        }

        public void Close()
        {
            isOpen = false;
            mesh.Clear();
            vertices.Clear();
            if (renderer != null)
                renderer.enabled = false;

            if (spriteRenderer != null)
                spriteRenderer.sprite = closeSprite;

            if (particleSystem != null)
                particleSystem.gameObject.SetActive(false);

            if (closeSystem != null)
                closeSystem.gameObject.SetActive(true);
        }


        public bool IsOpened()
        {
            return isOpen;
        }

        public void Lock()
        {
            isLocked = true;
        }

        public void Unlock()
        {
            isLocked = false;
        }

        public bool IsLocked()
        {
            return isLocked;
        }
        
        public bool StateIsPermanentlyLocked() => permanentlyLocked;
        
        public void PermanentlyLock()
        {
            isLocked = true;
            permanentlyLocked = true;
        }
    }
}