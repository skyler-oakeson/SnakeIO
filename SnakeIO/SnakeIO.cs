using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Scenes;

namespace SnakeIO
{
    public class SnakeIO : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Shared.DataManager dataManager;
        private Shared.Controls.ControlManager controlManager;
        private Shared.Controls.ControlManager contentManager;
        private GameScene gameView;

        public SnakeIO()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            dataManager = new Shared.DataManager();
            controlManager = new Shared.Controls.ControlManager(dataManager);
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
            MessageQueueClient.instance.initialize("localhost", 3000);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                MessageQueueClient.instance.sendMessage(new Shared.Messages.Disconnect());
                MessageQueueClient.instance.shutdown();
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
