using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
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
        GetComponent<HitSensor>().OnHit += HandleCollision;
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

    private void HandleCollision(HitStimulus other)
    {
        if (other.tag=="Player"&&this.GetComponent<HitStimulus>().DamageSource==HitStimulus.DamageSourceType.Ennemy)
        {
            Destroy(this.gameObject);
        }
        if (other.tag=="Enemy"&&this.GetComponent<HitStimulus>().DamageSource==HitStimulus.DamageSourceType.William)
        {
            Destroy(this.gameObject);
        }
    }


}
