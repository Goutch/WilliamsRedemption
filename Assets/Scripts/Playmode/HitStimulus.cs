using System;
using System.Collections;
using System.Collections.Generic;
using Harmony;
using UnityEditor;
using UnityEngine;

public class HitStimulus : MonoBehaviour
{
    private HitSensor hitSensor;

    private void OnCollisionEnter2D(Collision2D other)
    {
        hitSensor = null;

        hitSensor = other.gameObject.GetComponent<HitSensor>();


        if (hitSensor != null)
        {
            if (other.collider.tag == "ProjectilePlayer" || other.collider.tag == "ProjectileEnemy")
                hitSensor.Hit(this, other.collider.GetComponent<ProjectileController>().EntityData);
            else
            {
                hitSensor.Hit(this, other.collider.GetComponent<ProjectileController>().EntityData);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        hitSensor = null;
        hitSensor = other.gameObject.GetComponent<HitSensor>();
        if (other.tag == "ProjectilePlayer" && gameObject.tag == "Enemy"
            || other.tag == "ProjectileEnemy" && gameObject.tag == "Player")
        {
            hitSensor.Hit(this, other.GetComponent<ProjectileController>().EntityData);
            Destroy(other.gameObject);
        }
        else
        {
            hitSensor.Hit(this,other.GetComponent<IEntityData>());
        }
    }
}
