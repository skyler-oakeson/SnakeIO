using System;

namespace Scenes
{
    [Flags]
    public enum SceneContext : UInt16
    {
        MainMenu,
        Register,
        Game,
        Options,
        Scores,
        Credits,
        Exit,
    }
}
