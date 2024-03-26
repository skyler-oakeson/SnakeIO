using Microsoft.Xna.Framework;

namespace Systems
{
    public class KeyboardInput : System
    {
        Controls.ControlManager cm;
        Scenes.SceneContext sc;
        public KeyboardInput(Controls.ControlManager cm, Scenes.SceneContext sc)
            : base(
                   typeof(Components.KeyboardControllable)
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
