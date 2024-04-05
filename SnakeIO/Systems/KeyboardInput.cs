using Microsoft.Xna.Framework;

namespace Systems
{
    /// <summary>
    /// This system is responsible for handling the keyboard input of any
    /// entity with KeyboardControllable component.
    /// </summary>
    public class KeyboardInput : Shared.Systems.System
    {
        Shared.Controls.ControlManager cm;
        Scenes.SceneContext sc;
        public KeyboardInput(Shared.Controls.ControlManager cm, Scenes.SceneContext sc)
            : base(
                   typeof(Shared.Components.KeyboardControllable)
                   )
        {
            this.cm = cm;
            this.sc = sc;
        }

        public override void Update(GameTime gameTime)
        {
            cm.Update(gameTime, sc);
        }

    }
}
