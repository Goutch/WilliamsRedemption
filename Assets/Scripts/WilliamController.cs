using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WilliamController : EntityControlableController
{
    [SerializeField] private float dashForce;
    [SerializeField] private float durationOfDash;

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
            data.RigidBody.velocity = new Vector2(Vector2.right.x * (sprite.flipX ? dashForce * -1 : dashForce), 0.0f) * Time.deltaTime;

            yield return null;
        }
        data.RigidBody.velocity = new Vector2(0, data.RigidBody.velocity.y);

        data.IsDashing = false;
    }
}
