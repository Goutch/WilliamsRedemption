using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {

    [SerializeField] private float speed;
    [SerializeField] private float delayBeforeDestruction;

    public IEntityData EntityData { get; set; }

    private void Awake()
    {
        StartCoroutine(Destroy());
    }

    void FixedUpdate ()
    {
        transform.Translate(speed*Time.deltaTime,0,0);
	}

    private IEnumerator Destroy()
    {
        yield return new WaitForSecondsRealtime(delayBeforeDestruction);
        Destroy(this.gameObject);
    }
}
