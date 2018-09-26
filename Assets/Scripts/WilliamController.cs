using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WilliamController : EntityControlableController
{
    [SerializeField] private float dashForce;
    [SerializeField] private float durationOfDash;

    [SerializeField] private GameObject projectile;

    protected bool capacityUsedOnceInAir = false;

    public override void UseCapacity1(IPlayerData data)
    {
        StartCoroutine(Dash(data));
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

    IEnumerator Dash(IPlayerData data)
    {
        data.IsDashing = true;
        float numbOfTimePassed = 0;

        while (numbOfTimePassed < durationOfDash)
        {
            numbOfTimePassed += Time.deltaTime;
            data.RigidBody.velocity = new Vector2(Vector2.right.x * (sprite.flipX ? dashForce * -1 : dashForce), 0.0f);

            yield return null;
        }
        data.RigidBody.velocity = new Vector2(0, data.RigidBody.velocity.y);

        data.IsDashing = false;
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

        GameObject projectileObject = Instantiate(projectile, gameObject.transform.parent.position, angle);
        projectile.GetComponent<ProjectileController>().EntityData = (playerData as IPlayerDataReadOnly).Clone();
    }
}
