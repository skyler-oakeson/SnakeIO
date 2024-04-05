using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Scenes
{
    public class GameScene : Scene
    {
        private SnakeIO.GameModel gameModel;
        public GameScene(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics, Shared.Controls.ControlManager controlManager)
        {
            this.Initialize(graphicsDevice, graphics, controlManager);
            this.gameModel = new SnakeIO.GameModel(screenHeight, screenWidth);
        }

        override public void LoadContent(ContentManager contentManager)
        {
            gameModel.Initialize(controlManager, spriteBatch, contentManager);
        }

        override public SceneContext ProcessInput(GameTime gameTime)
        {
            return SceneContext.Game;
        }

        override public void Render(GameTime gameTime)
        {
            gameModel.Render(gameTime);
        }

        override public void Update(GameTime gameTime)
        {
            gameModel.Update(gameTime);
        }
    }
}

