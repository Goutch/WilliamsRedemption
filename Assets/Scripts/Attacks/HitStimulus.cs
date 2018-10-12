﻿using Harmony;
using Playmode.EnnemyRework;
using UnityEngine;
using Edgar;

public class HitStimulus : MonoBehaviour
{
    public enum DamageSourceType
    {
        Reaper,
        William,
        Enemy,
        Obstacle,
        None,
    }

    [SerializeField] private DamageSourceType damageSource;

    public DamageSourceType DamageSource => damageSource;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.collider.GetComponent<IgnoreStimulus>())
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
            damageSource = DamageSourceType.Enemy;
        }
    }

    public void SetDamageSource(DamageSourceType newType)
    {
        damageSource = newType;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.GetComponent<IgnoreStimulus>())
        {
            HitSensor hitSensor = other.Root().GetComponent<HitSensor>();

            if (hitSensor != null)
            {
                hitSensor.Hit(this);
            }
        }
    }
}

