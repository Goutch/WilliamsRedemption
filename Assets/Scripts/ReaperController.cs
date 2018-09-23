using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaperController : EntityControlableController
{
    [SerializeField] private float teleportationDistance;

    public override void UseCapacity1(IPlayerData data)
    {
        Transform root = transform.parent;

        RaycastHit2D raycast = Physics2D.Raycast(root.position, Vector3.right * (sprite.flipX ? -1 : 1), teleportationDistance * Time.deltaTime, LayerMask.NameToLayer("Default"));

        root.position = root.position + Vector3.right * (sprite.flipX ? -1 : 1) * teleportationDistance * Time.deltaTime;
    }

    public override bool Capacity1Usable(IPlayerDataReadOnly data)
    {
        Transform root = transform.parent;
        bool colliding = false;

        RaycastHit2D hit =
                    Physics2D.Raycast(this.transform.position, Vector3.right * (sprite.flipX ? -1 : 1), teleportationDistance * Time.deltaTime, 1 << LayerMask.NameToLayer("Obstacles"));

        if (hit.collider == null)
        {
            Debug.DrawRay(root.position, Vector3.right * (sprite.flipX ? -1 : 1) * teleportationDistance * Time.deltaTime, Color.yellow);
            colliding = true;
        }
        else
        {
            Debug.DrawRay(root.position, Vector3.right * (sprite.flipX ? -1 : 1) * hit.distance, Color.red);
        }

        return colliding && data.IsOnGround;
    }
}
