using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Entities;
using Systems;
using Controls;

namespace Scenes
{
    public class MainMenuScene : Scene
    {
        private Renderer<SpriteFont> renderer;
        private KeyboardInput keyboardInput;
        private Audio audio;
        private Linker linker;

        public MainMenuScene(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics, Controls.ControlManager controlManager)
        {
            this.Initialize(graphicsDevice, graphics, controlManager);

            this.controlManager = controlManager;
            this.keyboardInput = new Systems.KeyboardInput(controlManager);
            this.renderer = new Renderer<SpriteFont>(spriteBatch);
            this.audio = new Audio();
            this.linker = new Linker();
        }

        override public void LoadContent(ContentManager contentManager)
        {
            SpriteFont font = contentManager.Load<SpriteFont>("Fonts/Micro5-50");
            SoundEffect sound = contentManager.Load<SoundEffect>("Audio/click");
            AddEntity(MenuItem.Create(font, "Test", true, new Vector2(50, 50), sound, Components.LinkPosition.Head, controlManager));
            AddEntity(MenuItem.Create(font, "Check", false, new Vector2(100, 100), sound, Components.LinkPosition.Tail, controlManager));
            AddEntity(MenuItem.Create(font, "Check", false, new Vector2(100, 100), sound, Components.LinkPosition.Body, controlManager));
        }

        override public SceneContext ProcessInput(GameTime gameTime)
        {
            return SceneContext.MainMenu;
        }

        override public void Render(GameTime gameTime)
        {
            renderer.Update(gameTime);
        }

        override public void Update(GameTime gameTime)
        {
            renderer.Update(gameTime);
            keyboardInput.Update(gameTime);
            audio.Update(gameTime);
            linker.Update(gameTime);
        }

        private void AddEntity(Entity entity)
        {
            renderer.Add(entity);
            keyboardInput.Add(entity);
            audio.Add(entity);
            linker.Add(entity);
        }

        private void RemoveEntity(Entity entity)
        {
            renderer.Remove(entity.id);
            keyboardInput.Remove(entity.id);
            audio.Remove(entity.id);
            linker.Remove(entity.id);
        }

    }
}
