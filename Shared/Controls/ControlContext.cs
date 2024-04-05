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
        MoveTowards,
        MenuUp,
        MenuDown,
    }
}
