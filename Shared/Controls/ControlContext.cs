using System;

namespace Shared.Controls
{
    [Flags]
    public enum ControlContext : UInt16
    {
        MoveLeft,
        MoveRight,
        MoveUp,
        MoveDown,
        MoveTowards,
        MenuUp,
        MenuDown,
        Confirm,
        MouseClick,
        MouseDown,
        MouseUp,
        MouseMove,
    }
}
