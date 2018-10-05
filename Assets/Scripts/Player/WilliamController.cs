﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WilliamController : EntityControlableController
{
    [Tooltip("Distance travelled by the player during a dash.")]
    [SerializeField] private float dashDistance;
    
    [Tooltip("Speed at witch the player dashes.")]
    [SerializeField] private float dashSpeed;
    
    [SerializeField] private GameObject projectile;
    
    [Tooltip("Amount of time between bullets.")]
    [SerializeField] private float fireRate;
    
    [Tooltip("Amount of time between dashes.")]
    [SerializeField] private float DashCoolDown;

    private bool capacityCanBeUsed = false;
    private float? lastTimeAttack = null;
    private float timerStartTime;

    private void Start()
    {
        timerStartTime = 0;
        capacityCanBeUsed = true;
    }

    public override void UseCapacity(PlayerController player , Vector2 direction)
    {
        StartCoroutine(Dash(player , direction));
        capacityCanBeUsed = false;
        timerStartTime = Time.time;
        
    }

    public override bool CapacityUsable(PlayerController player)
    {
        if (capacityCanBeUsed)
        {
            return true;
        }
        if (!capacityCanBeUsed && (Time.time - timerStartTime) >= DashCoolDown)
        {
            capacityCanBeUsed = true;
            return true;
        }
        return false;
    }

    IEnumerator Dash(PlayerController player , Vector2 direction)
    {
        Debug.Log("IsCalled");
        player.LockTransformation();
        player.IsDashing = true;
        
        Transform root = transform.parent;
        
        RaycastHit2D hit =
            Physics2D.Raycast(
                root.position,
                direction, dashDistance,
                player.WilliamLayerMask);
        Debug.DrawLine(root.position ,new Vector3(root.position.x + dashDistance*direction.x ,root.position.y, root.position.z), Color.yellow,10);
             
        if (hit.collider == null)
        {     
            hit.point= new Vector2(dashDistance*direction.x + transform.position.x , transform.position.y);
        }

        float distance = Vector2.Distance(hit.point, transform.position);
        float duration = distance/dashSpeed;

        float time=0;

       
        while(duration > time) 
        {
            time += Time.deltaTime;
            player.kRigidBody.Velocity = Vector2.right * direction.x * dashSpeed; //set our rigidbody velocity to a custom velocity every frame.
            yield return 0;
        }
      
        player.IsDashing = false;
        player.UnlockTransformation();
    }

    public override bool CanUseBasicAttack(PlayerController player)
    {
        if(lastTimeAttack == null || Time.time - lastTimeAttack > fireRate)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public override void UseBasicAttack(PlayerController player ,Vector2 direction)
    {

        Quaternion angle = Quaternion.identity;

        if (direction == Vector2.left)
            angle = Quaternion.AngleAxis(180, Vector3.up);

        if (direction == Vector2.down && !player.IsOnGround)
            angle = Quaternion.AngleAxis(-90, Vector3.forward);
        else if (direction == Vector2.up)
            angle = Quaternion.AngleAxis(90, Vector3.forward);

        GameObject projectileObject = Instantiate(projectile, gameObject.transform.position, angle);
        projectileObject.GetComponent<HitStimulus>().SetDamageSource(HitStimulus.DamageSourceType.William);

        Attacking = true;

        lastTimeAttack = Time.time;
    }
}
