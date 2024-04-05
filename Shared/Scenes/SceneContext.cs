using System;

namespace Scenes
{
    [Flags]
    public enum SceneContext : UInt16
    {
        MainMenu,
        Game,
        Options,
        Scores,
        Credits,
        Exit,
    }
}
