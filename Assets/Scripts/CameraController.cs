using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField] private Transform followPoint;
    [SerializeField] private float positionZ;
    private bool follow = true;
    private float initialSize;

    public bool Follow
    {
        get
        {
            return follow;
        }

        set
        {
            follow = value;
        }
    }

    private void Awake()
    {
        initialSize = Camera.main.orthographicSize;
    }

    private void Update()
    {
        if(follow&&followPoint!=null)
        {
            Camera.main.transform.position = new Vector3(followPoint.position.x, followPoint.position.y, positionZ);
        }
    }

    public void FixePoint(Vector2 point, float size)
    {
        Camera.main.transform.position = new Vector3(point.x, point.y, positionZ);
        Camera.main.orthographicSize = size;
        follow = false;
    }

    public void ResumeFollow()
    {
        follow = true;
        Camera.main.orthographicSize = initialSize;
    }
}
