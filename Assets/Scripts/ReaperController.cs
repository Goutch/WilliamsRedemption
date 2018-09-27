using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaperController : EntityControlableController
{
    [SerializeField] private float teleportationDistance;
    [SerializeField] private GameObject tpEffect;
    [SerializeField] private float timeBeforeTpEffectVanish;

    public override void UseCapacity1(IPlayerData data)
    {
        Transform root = transform.parent;
        Vector3 direction = (sprite.flipX ? Vector3.left : Vector3.right);

        GameObject tpEffectTemp = Instantiate(tpEffect, root.position, Quaternion.identity);
        StartCoroutine(TeleportEffectRemove(tpEffectTemp));

        root.position = root.position + direction * teleportationDistance * Time.deltaTime;
    }

    public override bool Capacity1Usable(IPlayerDataReadOnly data)
    {
        Transform root = transform.parent;
        Vector3 direction = (sprite.flipX ? Vector3.left : Vector3.right);
        bool colliding = false;

        RaycastHit2D hit =
                    Physics2D.Raycast(
                        root.position,
                        direction, teleportationDistance * Time.deltaTime,
                        1 << LayerMask.NameToLayer("Obstacles"));

        if (hit.collider == null)
        {
            Debug.DrawRay(root.position, direction * teleportationDistance * Time.deltaTime, Color.yellow);
            colliding = true;
        }
        else
        {
            Debug.DrawRay(root.position, direction * hit.distance, Color.red);
        }

        return colliding && data.IsOnGround;
    }

    IEnumerator TeleportEffectRemove(GameObject tpEffect)
    {
        yield return new WaitForSeconds(timeBeforeTpEffectVanish);
        Destroy(tpEffect);
    }
}
