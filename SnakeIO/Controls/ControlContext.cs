using System;

namespace Controls
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
