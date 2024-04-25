using System;

namespace Scenes
{
    [Flags]
    public enum SceneContext : UInt16
    {
        MainMenu,
        Name,
        Hud,
        Game,
        GameOver,
        Options,
        Scores,
        Credits,
        Exit,
    }
}
