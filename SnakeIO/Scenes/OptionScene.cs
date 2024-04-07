using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Components;
using Systems;
using Entities;

namespace Scenes
{
    public class OptionScene : Scene
    {
        private Renderer<SpriteFont> renderer;
        private KeyboardInput keyboardInput;
        private Selector<Controls.Control> selector;
        private Audio audio;
        private Linker linker;

        public OptionScene(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics, Controls.ControlManager controlManager)
        {
            this.Initialize(graphicsDevice, graphics, controlManager);

            this.controlManager = controlManager;
            this.keyboardInput = new Systems.KeyboardInput(controlManager);
            this.selector = new Systems.Selector<Controls.Control>();
            this.renderer = new Renderer<SpriteFont>(spriteBatch);
            this.audio = new Audio();
            this.linker = new Linker();
        }

        override public void LoadContent(ContentManager contentManager)
        {
            SpriteFont font = contentManager.Load<SpriteFont>("Fonts/Micro5-50");
            SoundEffect sound = contentManager.Load<SoundEffect>("Audio/click");
            AddEntity(MenuItem<Controls.Control>.Create(
                        font, controlManager.GetControl(Controls.ControlContext.MoveUp), 
                        "options", true, new Vector2(50, 50), sound, Components.LinkPosition.Head, controlManager));
            AddEntity(MenuItem<Controls.Control>.Create(
                        font, controlManager.GetControl(Controls.ControlContext.MoveDown),
                        "options", false, new Vector2(50, 100), sound, Components.LinkPosition.Body, controlManager));
            AddEntity(MenuItem<Controls.Control>.Create(
                        font, controlManager.GetControl(Controls.ControlContext.MoveLeft),
                        "options",  false, new Vector2(50, 150), sound, Components.LinkPosition.Body, controlManager));
            AddEntity(MenuItem<Controls.Control>.Create(
                        font, controlManager.GetControl(Controls.ControlContext.MoveRight),
                        "options",  false, new Vector2(50, 200), sound, Components.LinkPosition.Tail, controlManager));
        }

        override public SceneContext ProcessInput(GameTime gameTime)
        {
            selector.Update(gameTime);
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                return SceneContext.MainMenu;
            }
            return SceneContext.Options;
        }

        override public void Render(GameTime gameTime)
        {
            renderer.Update(gameTime);
        }

        override public void Update(GameTime gameTime)
        {
            renderer.Update(gameTime);
            selector.Update(gameTime);
            keyboardInput.Update(gameTime);
            audio.Update(gameTime);
            linker.Update(gameTime);
        }

        private void AddEntity(Entity entity)
        {
            renderer.Add(entity);
            selector.Add(entity);
            keyboardInput.Add(entity);
            audio.Add(entity);
            linker.Add(entity);
        }

        private void RemoveEntity(Entity entity)
        {
            renderer.Remove(entity.id);
            selector.Remove(entity.id);
            keyboardInput.Remove(entity.id);
            audio.Remove(entity.id);
            linker.Remove(entity.id);
        }

    }
}
