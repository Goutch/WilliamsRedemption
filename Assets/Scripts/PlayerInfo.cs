using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerData : IPlayerData
{
    public bool IsOnGround { get; set; } = false;
    public bool IsDashing { get; set; } = false;
    public Rigidbody2D RigidBody { get; set; }
}

