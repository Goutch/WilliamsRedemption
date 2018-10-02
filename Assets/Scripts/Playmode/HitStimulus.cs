using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Harmony;
using Playmode.EnnemyRework;
using UnityEditor;
using UnityEngine;

public class HitStimulus : MonoBehaviour
{
    public enum DamageSourceType
    {
        Reaper,
        William,
        Ennemy,
        Obstacle,
        None,
        
    }

    private DamageSourceType damageSource;

    public DamageSourceType DamageSource => damageSource;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.GetComponent<IgnoreStimulus>() == null)
        {
            HitSensor hitSensor = other.collider.Root().GetComponent<HitSensor>();

            if (hitSensor != null)
            {
                hitSensor.Hit(this);
            }
        }
    }

    private void Awake()
    {
        if(GetComponent<Enemy>())
        {
            damageSource = DamageSourceType.Ennemy;
        }
        else
        {
            damageSource= DamageSourceType.None;
        }
    }

    public void SetDamageSource(DamageSourceType newType)
    {
        damageSource = newType;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<IgnoreStimulus>() == null)
        {
            HitSensor hitSensor = other.Root().GetComponent<HitSensor>();
            if (hitSensor != null)
            {
                hitSensor.Hit(this);
            }
        }

       // if (other.tag == "ProjectilePlayer" && gameObject.tag == "Enemy"
       //     || other.tag == "ProjectileEnemy" && gameObject.tag == "Player")
       // {
       //     gameObject.Hit(other);
       //     Destroy(other.gameObject);
       // }
//
       // if (other.gameObject.GetComponent<PlasmaController>() != null)
       // {
       //     if (other.tag == "ProjectileEnemy" && gameObject.tag == "ProjectilePlayer"
       //                                        && other.gameObject.GetComponent<ProjectileController>()
       //                                            .CanBeReturned ==
       //                                        true)
       //     {
       //         other.gameObject.GetComponent<PlasmaController>().ChangeDirection();
       //     }
       //     else if (other.gameObject.GetComponent<PlasmaController>().CanBulletKillHisShooter() == true
       //              && other.tag == "ProjectileEnemy" && gameObject.tag == "Enemy")
       //     {
       //         gameObject.GetComponentInChildren<HitSensor>().Hit();
       //         Destroy(other.gameObject);
       //     }
       // }
    }
}

