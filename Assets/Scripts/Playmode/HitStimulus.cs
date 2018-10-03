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
    }
}

