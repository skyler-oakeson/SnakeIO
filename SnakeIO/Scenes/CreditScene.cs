using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using Microsoft.Xna.Framework.Input;

namespace Scenes
{
    public class CreditScene : Scene
    {
        private Systems.Renderer renderer;
        private Systems.KeyboardInput keyboardInput;
                private Systems.Audio audio;
        private ContentManager contentManager;
        public CreditScene(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics, Shared.Controls.ControlManager controlManager)
        {
           
            this.Initialize(graphicsDevice, graphics, controlManager);
            this.controlManager = controlManager;
            this.keyboardInput = new Systems.KeyboardInput(controlManager);
            this.renderer = new Systems.Renderer(spriteBatch);
            this.audio = new Systems.Audio();

        }

        override public void LoadContent(ContentManager contentManager)
        {

        }

        override public SceneContext ProcessInput(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                return SceneContext.MainMenu;
            }

            return SceneContext.Credits;
        }

        override public void Render(TimeSpan elapsedTime)
        {

        }

        override public void Update(TimeSpan elapsedTime)
        {

        }

    }
}

