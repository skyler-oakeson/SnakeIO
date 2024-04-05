using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Scenes
{
    public class ScoreScene : Scene
    {

        public ScoreScene(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics, Shared.Controls.ControlManager controlManager)
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

        override public void Render(GameTime gameTime)
        {

        }

        override public void Update(GameTime gameTime)
        {

        }

    }
}

