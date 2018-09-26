using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaperController : EntityControlableController
{
    public override void UseCapacity1(IPlayerData data)
    {
    }
    public override bool Capacity1Usable(IPlayerDataReadOnly data)
    {
        return true;
    }

    public override bool CanUseBasicAttack(IPlayerDataReadOnly playerData)
    {
        throw new System.NotImplementedException();
    }

    public override void UseBasicAttack(IPlayerData playerData)
    {
        throw new System.NotImplementedException();
    }
}
