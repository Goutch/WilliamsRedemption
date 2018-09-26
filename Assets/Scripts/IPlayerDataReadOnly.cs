using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IPlayerDataReadOnly : IEntityData
{
    bool IsOnGround { get; }
    bool IsDashing { get; }

    IPlayerDataReadOnly Clone();
}
