using System;

namespace Shared.Controls
{
    [Flags]
    public enum ControlContext 
    {
        MoveLeft,
        MoveRight,
        MoveUp,
        MoveDown,
        MenuUp,
        MenuDown,
    }
}
