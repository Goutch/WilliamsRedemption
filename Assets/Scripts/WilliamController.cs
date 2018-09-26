using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WilliamController : EntityControlableController
{
    [SerializeField] private float dashForce;
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

        float numbOfTimePassed = 0;
        while (numbOfTimePassed < durationOfDash)
        {
            numbOfTimePassed += Time.deltaTime;
            player.Rigidbody.velocity = new Vector2(Vector2.right.x * (sprite.flipX ? dashForce * -1 : dashForce), 0.0f) * Time.deltaTime;

            yield return null;
        }
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

        GameObject projectileObject = Instantiate(projectile, gameObject.transform.parent.position, angle);
        projectile.GetComponent<ProjectileController>().EntityData = (playerData as IPlayerDataReadOnly).Clone();
    }
}
