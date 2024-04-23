using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using Microsoft.Xna.Framework.Input;
using Systems;
using Microsoft.Xna.Framework.Audio;
using Shared.Systems;
using Shared.Entities;

namespace Scenes
{
    public class ScoreScene : Scene
    {
        private Renderer renderer;
        private KeyboardInput keyboardInput;
        private Selector<SceneContext> selector;
        private Audio audio;

        public ScoreScene(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics, Shared.Controls.ControlManager controlManager)
        {
            this.Initialize(graphicsDevice, graphics, controlManager);
            this.controlManager = controlManager;
            this.keyboardInput = new Systems.KeyboardInput(controlManager);
            this.selector = new Systems.Selector<SceneContext>();
            this.renderer = new Renderer(spriteBatch);
            this.audio = new Audio();
        }

        override public void LoadContent(ContentManager contentManager)
        {

            int center = graphics.PreferredBackBufferWidth / 2;
            SpriteFont font = contentManager.Load<SpriteFont>("Fonts/Micro5-50");
            SoundEffect sound = contentManager.Load<SoundEffect>("Audio/click");
            AddEntity(Shared.Entities.Score.create(new Rectangle(center - (int)font.MeasureString("Scores").X/2, 50, 0, 0), font, "Scores"));

        }

        override public SceneContext ProcessInput(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                return SceneContext.MainMenu;
            }

            return SceneContext.Scores;
        }

        override public void Render(TimeSpan elapsedTime)
        {
            renderer.Update(elapsedTime);
        }

        private void AddEntity(Shared.Entities.Entity entity)
        {
            renderer.Add(entity);
            selector.Add(entity);
            keyboardInput.Add(entity);
            audio.Add(entity);
            
        }


        override public void Update(TimeSpan elapsedTime)
        {
            renderer.Update(elapsedTime);
            selector.Update(elapsedTime);
            keyboardInput.Update(elapsedTime);
            audio.Update(elapsedTime);

        }

    }
}

