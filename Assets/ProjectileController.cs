using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {

    [SerializeField] private float speed;
    [SerializeField] private float delayBeforeDestruction;
    [SerializeField] private bool canBeReturned;
    
    protected int direction;
    public IEntityData EntityData { get; set; }
    public bool CanBeReturned
    {
        get { return canBeReturned;}
        private set { canBeReturned=value; }
    }
    
    protected virtual void Awake()
    {
        StartCoroutine(Destroy());
        direction = 1;
    }
    void FixedUpdate ()
    {
        transform.Translate(speed*Time.deltaTime*direction,0,0);
	}
    private IEnumerator Destroy()
    {
        yield return new WaitForSecondsRealtime(delayBeforeDestruction);
        Destroy(this.gameObject);
    }


}
