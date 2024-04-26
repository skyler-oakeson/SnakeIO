using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using Microsoft.Xna.Framework.Input;
using Systems;
using System.Collections.Generic;


namespace Scenes
{
    public class CreditScene : Scene
    {
        private Renderer renderer;
        private KeyboardInput keyboardInput;
        private Selector<SceneContext> selector;
        private Audio audio;
        private SpriteFont font;

        public CreditScene(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics, Shared.Controls.ControlManager controlManager)
        {
            this.Initialize(graphicsDevice, graphics, controlManager);
            this.controlManager = controlManager;
            this.keyboardInput = new Systems.KeyboardInput(controlManager);
            this.renderer = new Renderer(spriteBatch);
            this.audio = new Audio();

        }

        override public void LoadContent(ContentManager contentManager)
        {
            int center = graphics.PreferredBackBufferWidth / 2;
            font = contentManager.Load<SpriteFont>("Fonts/Micro5-50");
            AddEntity(Shared.Entities.StaticText.Create(font, "Credits", Color.Black, Color.Orange, new Rectangle(center - (int)font.MeasureString("Credits").X / 2, 50 + (int)font.MeasureString("Credits").Y / 2, 0, 0)));
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
            renderer.Update(elapsedTime);
        }

        override public void Update(TimeSpan elapsedTime)
        {

            renderer.Update(elapsedTime);
            keyboardInput.Update(elapsedTime);
            audio.Update(elapsedTime);
        }

        private void AddEntity(Shared.Entities.Entity entity)
        {
            renderer.Add(entity);
            keyboardInput.Add(entity);
            audio.Add(entity);

        }



    }
}

