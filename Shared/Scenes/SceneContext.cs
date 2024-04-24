using System;

namespace Scenes
{
    [Flags]
    public enum SceneContext : UInt16
    {
        MainMenu,
        Name,
        Game,
        Options,
        Scores,
        Credits,
        Exit,
    }
}
