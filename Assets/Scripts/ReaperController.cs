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
}
