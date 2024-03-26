using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Controls;
using Scenes;

namespace Yew 
{
    public class SnakeIO : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private DataManager dataManager;
        private ControlManager controlManager;
        private ControlManager contentManager;
        private GameScene gameView;

        public SnakeIO()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            dataManager = new DataManager();
            controlManager = new ControlManager(dataManager);
        }

        protected override void Initialize()
        {
            gameView = new GameScene(graphics.GraphicsDevice, graphics, controlManager);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            gameView.LoadContent(this.Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            gameView.Update(gameTime);


            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            gameView.Render(gameTime);

            base.Draw(gameTime);
        }
    }
}
