using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using Harmony;
using UnityEngine;

public class ProjectileController : MonoBehaviour {

    [SerializeField] private float speed;
    [SerializeField] private float delayBeforeDestruction;
    [SerializeField] private bool canBeReturned;
    
    protected int direction;
    public bool CanBeReturned
    {
        get { return canBeReturned;}
        private set { canBeReturned=value; }
    }

    public float Speed { get { return speed; } set { speed = value; } }

    protected virtual void Awake()
    {
        StartCoroutine(Destroy());
        direction = 1;
        GetComponent<HitSensor>().OnHit += HandleCollision;
    }
    void FixedUpdate ()
    {
        transform.Translate(Speed*Time.deltaTime*direction,0,0);
	}
    private IEnumerator Destroy()
    {
        yield return new WaitForSecondsRealtime(delayBeforeDestruction);
        Destroy(this.gameObject);
    }

    protected virtual void HandleCollision(HitStimulus other)
    {
        if (CanBeReturned&&other.GetComponent<HitStimulus>().DamageSource == HitStimulus.DamageSourceType.Reaper &&other.GetComponent<MeleeAttackController>() )
        {
            this.GetComponent<HitStimulus>().SetDamageSource(other.GetComponent<HitStimulus>().DamageSource);
            direction *= -1;
        }
        else if (other.tag=="Player"&&this.GetComponent<HitStimulus>().DamageSource==HitStimulus.DamageSourceType.Enemy)
        {
            Destroy(this.gameObject);
        }
        else if (other.tag=="Enemy"&&this.GetComponent<HitStimulus>().DamageSource==HitStimulus.DamageSourceType.William)
        {
            Destroy(this.gameObject);
        }
        else if(other.tag=="Enemy"&&this.GetComponent<HitStimulus>().DamageSource==HitStimulus.DamageSourceType.Reaper)
        {
            Destroy(this.gameObject);
        }
    }


}
