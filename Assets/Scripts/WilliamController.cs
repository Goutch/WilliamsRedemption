﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WilliamController : EntityControlableController
{
    [SerializeField] private float dashDistance;
    [SerializeField] private float durationOfDash;

    [SerializeField] private GameObject projectile;

    protected bool capacityUsedOnceInAir = false;

    public override void UseCapacity1(PlayerController player)
    {
        StartCoroutine(Dash(player));
        capacityUsedOnceInAir = true;
    }

    public override bool Capacity1Usable(IPlayerDataReadOnly data)
    {
        if (data.IsOnGround)
            capacityUsedOnceInAir = false;

        if (capacityUsedOnceInAir)
            return false;
        else
            return true;
    }

    IEnumerator Dash(PlayerController player)
    {
        player.LockTransformation();
        player.IsDashing = true;

        Vector3 direction = (sprite.flipX ? Vector3.left : Vector3.right);
        player.Rigidbody.velocity = new Vector2(direction.x * (dashDistance / durationOfDash), player.Rigidbody.velocity.y);

        yield return new WaitForSeconds(durationOfDash);

        player.Rigidbody.velocity = new Vector2(0, player.Rigidbody.velocity.y);
        player.IsDashing = false;
        player.UnlockTransformation();
    }

    public override bool CanUseBasicAttack(IPlayerDataReadOnly playerData)
    {
        return true;
    }

    public override void UseBasicAttack(IPlayerData playerData)
    {
        Quaternion angle = Quaternion.identity;

        if (sprite.flipX)
            angle = Quaternion.AngleAxis(180, Vector3.up);

        if (playerData.DirectionFacingUpDown == FacingSideUpDown.Down)
            angle = Quaternion.AngleAxis(-90, Vector3.forward);
        else if (playerData.DirectionFacingUpDown == FacingSideUpDown.Up)
            angle = Quaternion.AngleAxis(90, Vector3.forward);

        GameObject projectileObject = Instantiate(projectile, gameObject.transform.position, angle);
        projectile.GetComponent<ProjectileController>().EntityData = (playerData as IPlayerDataReadOnly).Clone();
    }
}
