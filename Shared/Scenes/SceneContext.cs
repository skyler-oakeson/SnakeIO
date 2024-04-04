using System;

namespace Scenes
{
    [Flags]
    public enum SceneContext 
    {
        MainMenu,
        Game,
        Options,
        Scores,
        Credits,
        Exit,
    }
}
