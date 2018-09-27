using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaperController : EntityControlableController
{
    [SerializeField] private float teleportationDistance;
    [SerializeField] private GameObject tpEffect;
    [SerializeField] private float timeBeforeTpEffectVanish;

    public override void UseCapacity1(PlayerController player)
    {
        Transform root = transform.parent;
        Vector3 direction = (sprite.flipX ? Vector3.left : Vector3.right);

        RaycastHit2D hit =
                    Physics2D.Raycast(
                        root.position,
                        direction, teleportationDistance * Time.deltaTime,
                        1);

        if (hit.collider == null)
        {
            root.Translate(direction * teleportationDistance * Time.deltaTime);
        }
        else
        {
            root.Translate(direction* hit.distance * Time.deltaTime);
        }

        GameObject tpEffectTemp = Instantiate(tpEffect, root.position, Quaternion.identity);
        StartCoroutine(TeleportEffectRemove(tpEffectTemp, player));
    }

    public override bool Capacity1Usable(IPlayerDataReadOnly data)
    { 
        return data.IsOnGround;
    }

    IEnumerator TeleportEffectRemove(GameObject tpEffect, PlayerController player)
    {
        player.LockTransformation();
        yield return new WaitForSeconds(timeBeforeTpEffectVanish);
        Destroy(tpEffect);
        player.UnlockTransformation();
    }

    public override bool CanUseBasicAttack(IPlayerDataReadOnly playerData)
    {
        return true;
    }

    public override void UseBasicAttack(IPlayerData playerData)
    {
        
    }
}
