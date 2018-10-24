using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ReaperController : EntityControlableController
{
    [Tooltip("Distance travelled by the player when teleporting.")]
    [SerializeField] private float teleportationDistance;
    [SerializeField] private GameObject tpEffect;
    [SerializeField] private GameObject meleeAttack;
    [Tooltip("Amount of time before the teleportation visual effect vanishes.")]
    [SerializeField] private float timeBeforeTpEffectVanish;
    
    [Tooltip("Amount of time between teleportations.")]
    [SerializeField] private float TeleportationCoolDown;

    private bool capacityCanBeUsed;
    private float timerStartTime;

    private void Start()
    {
        capacityCanBeUsed = true;
        timerStartTime = 0;
    }

    public override void UseCapacity(PlayerController player , Vector2 direction)
    {
        Transform root = transform.parent;
        GameObject tpEffectTemp = Instantiate(tpEffect, root.position, Quaternion.identity);
        StartCoroutine(TeleportEffectRemove(tpEffectTemp, player));
        
        //Vector3 direction = (sprite.flipX ? Vector3.left : Vector3.right);

        RaycastHit2D hit =
                    Physics2D.Raycast(
                        root.position,
                        direction, teleportationDistance * Time.deltaTime,
                        player.ReaperLayerMask);

        if (hit.collider == null)
        {
            root.Translate(direction * teleportationDistance * Time.deltaTime);
        }
        else
        {
            root.Translate(direction* hit.distance * Time.deltaTime);
        }
   
        capacityCanBeUsed = false;
        timerStartTime = Time.time;
    }

    public override bool CapacityUsable(PlayerController player)
    { 
        if (capacityCanBeUsed && player.IsOnGround)
        {
            return true;
        }
        if (!capacityCanBeUsed && (Time.time - timerStartTime) >= TeleportationCoolDown)
        {
            capacityCanBeUsed = true;
            if (player.IsOnGround)
            {
                return true;
            }    
        }
        return false;
    }

    IEnumerator TeleportEffectRemove(GameObject tpEffect, PlayerController player)
    {
        player.LockTransformation();
        yield return new WaitForSeconds(timeBeforeTpEffectVanish);
        Destroy(tpEffect);
        player.UnlockTransformation();
    }


    public override void UseBasicAttack(PlayerController player , Vector2 direction)
    {
        Quaternion angle = Quaternion.identity;

        if (direction == Vector2.left)
            angle = Quaternion.AngleAxis(180, Vector3.up);

        if (direction == Vector2.down && !player.IsOnGround)
            angle = Quaternion.AngleAxis(-90, Vector3.forward);
        else if (direction == Vector2.up)
            angle = Quaternion.AngleAxis(90, Vector3.forward);

        GameObject meleeAttackObject = Instantiate(meleeAttack, gameObject.transform.position, angle);
        meleeAttackObject.GetComponent<HitStimulus>().SetDamageSource(HitStimulus.DamageSourceType.Reaper);
        animator.SetTrigger("Attack");
        Attacking = true;
    }
    

    
}
