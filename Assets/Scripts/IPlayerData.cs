using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface IPlayerData : IPlayerDataReadOnly
{
    new bool IsOnGround { get; set; }
    new bool IsDashing { get; set; }
    new FacingSideUpDown DirectionFacingUpDown { get; set; }
    new FacingSideLeftRight DirectionFacingLeftRight { get; set; }
}
