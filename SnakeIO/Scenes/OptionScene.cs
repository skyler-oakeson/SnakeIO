using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Scenes
{
    public class OptionScene : Scene
    {

        public OptionScene(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics, Shared.Controls.ControlManager controlManager)
        {
            this.Initialize(graphicsDevice, graphics, controlManager);

        }

        override public void LoadContent(ContentManager contentManager)
        {

        }

        override public SceneContext ProcessInput(GameTime gameTime)
        {
            return SceneContext.Game;
        }

        override public void Render(TimeSpan elapsedTime)
        {

        }

        override public void Update(TimeSpan elapsedTime)
        {

        }

    }
}
