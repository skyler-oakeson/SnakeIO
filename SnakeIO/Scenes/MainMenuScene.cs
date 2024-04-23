using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Systems;

namespace Scenes
{
    public class MainMenuScene : Scene
    {
        private Renderer renderer;
        private KeyboardInput keyboardInput;
        private Selector<SceneContext> selector;
        private Audio audio;
        private Shared.Systems.Linker linker;

        public MainMenuScene(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics, Shared.Controls.ControlManager controlManager)
        {
            this.Initialize(graphicsDevice, graphics, controlManager);

            this.controlManager = controlManager;
            this.keyboardInput = new Systems.KeyboardInput(controlManager);
            this.selector = new Systems.Selector<SceneContext>();
            this.renderer = new Renderer(spriteBatch);
            this.audio = new Audio();
            this.linker = new Shared.Systems.Linker();
        }

        override public void LoadContent(ContentManager contentManager)
        {
            SpriteFont font = contentManager.Load<SpriteFont>("Fonts/Micro5-50");
            SoundEffect sound = contentManager.Load<SoundEffect>("Audio/click");
            AddEntity(Shared.Entities.MenuItem<SceneContext>.Create(
                        font, SceneContext.Game, "main", 
                        true, sound, Shared.Components.LinkPosition.Head, 
                        controlManager, new Rectangle(50, 50, 0, 0)));
            AddEntity(Shared.Entities.MenuItem<SceneContext>.Create(
                        font, SceneContext.Options, "main", false,
                        sound, Shared.Components.LinkPosition.Body,
                        controlManager, new Rectangle(50, 100, 0, 0)));
            AddEntity(Shared.Entities.MenuItem<SceneContext>.Create(
                        font, SceneContext.Scores, "main",
                        false, sound, Shared.Components.LinkPosition.Body,
                        controlManager, new Rectangle(50, 150, 0, 0)));
            AddEntity(Shared.Entities.MenuItem<SceneContext>.Create(
                        font, SceneContext.Exit, "main",
                        false, sound, Shared.Components.LinkPosition.Tail,
                        controlManager, new Rectangle(50, 200, 0, 0)));

        }

        override public SceneContext ProcessInput(GameTime gameTime)
        {
            selector.Update(gameTime.ElapsedGameTime);

            if (selector.selectedVal != default(SceneContext))
            {
                SceneContext selected = selector.selectedVal;
                selector.selectedVal = default(SceneContext);
                return selected;
            }

            return SceneContext.MainMenu;
        }

        override public void Render(TimeSpan elapsedTime)
        {
            renderer.Update(elapsedTime);
        }

        override public void Update(TimeSpan elapsedTime)
        {
            renderer.Update(elapsedTime);
            selector.Update(elapsedTime);
            keyboardInput.Update(elapsedTime);
            audio.Update(elapsedTime);
            linker.Update(elapsedTime);
        }

        private void AddEntity(Shared.Entities.Entity entity)
        {
            renderer.Add(entity);
            selector.Add(entity);
            keyboardInput.Add(entity);
            audio.Add(entity);
            linker.Add(entity);
        }

        private void RemoveEntity(Shared.Entities.Entity entity)
        {
            renderer.Remove(entity.id);
            selector.Remove(entity.id);
            keyboardInput.Remove(entity.id);
            audio.Remove(entity.id);
            linker.Remove(entity.id);
        }

    }
}
