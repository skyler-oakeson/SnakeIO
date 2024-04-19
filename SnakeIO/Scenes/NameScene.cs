using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Systems;

namespace Scenes
{
    public class NameScene : Scene
    {
        private Renderer renderer;
        private KeyboardInput keyboardInput;
        private Selector<string> selector;
        private Audio audio;
        private Shared.Systems.Linker linker;

        public NameScene(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics, Shared.Controls.ControlManager controlManager)
        {
            this.Initialize(graphicsDevice, graphics, controlManager);

            this.controlManager = controlManager;
            this.keyboardInput = new Systems.KeyboardInput(controlManager);
            this.selector = new Systems.Selector<string>();
            this.renderer = new Renderer(spriteBatch);
            this.audio = new Audio();
        }

        override public void LoadContent(ContentManager contentManager)
        {
            SpriteFont font = contentManager.Load<SpriteFont>("Fonts/Micro5-50");
            Texture2D background = contentManager.Load<Texture2D>("Images/text-input-bkg");
            SoundEffect sound = contentManager.Load<SoundEffect>("Audio/click");
            AddEntity(Shared.Entities.TextInput.Create(
                        font, background, "", 
                        true, new Rectangle(screenWidth/2, screenHeight/2, 0, 0)));
        }

        override public SceneContext ProcessInput(GameTime gameTime)
        {
            selector.Update(gameTime.ElapsedGameTime);

            return SceneContext.Name;
        }

        override public void Render(TimeSpan elapsedTime)
        {
            renderer.Update(elapsedTime);
        }

        override public void Update(TimeSpan elapsedTime)
        {
            selector.Update(elapsedTime);
            renderer.Update(elapsedTime);
            audio.Update(elapsedTime);
            keyboardInput.Update(elapsedTime);
        }

        private void AddEntity(Shared.Entities.Entity entity)
        {
            renderer.Add(entity);
            selector.Add(entity);
            keyboardInput.Add(entity);
            audio.Add(entity);
        }

        private void RemoveEntity(Shared.Entities.Entity entity)
        {
            renderer.Remove(entity.id);
            selector.Remove(entity.id);
            keyboardInput.Remove(entity.id);
            audio.Remove(entity.id);
        }

    }
}
