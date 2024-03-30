using Systems;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Entities;

namespace Scenes
{
    public class GameScene : Scene
    {
        private Renderer renderer;
        private KeyboardInput keyboardInput;
        private Movement movement;
        private Collision collision;
        private Audio audio;

        private List<Entity> removeThese = new List<Entity>();
        private List<Entity> addThese = new List<Entity>();

        public GameScene(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics, Controls.ControlManager controlManager)
        {
            this.Initialize(graphicsDevice, graphics, controlManager);

            this.keyboardInput = new Systems.KeyboardInput(controlManager, SceneContext.Game);
            this.movement = new Movement();
            this.renderer = new Renderer(spriteBatch);
            this.collision = new Collision();
            this.audio = new Audio();
        }

        override public void LoadContent(ContentManager contentManager)
        {
            Texture2D playerTex = contentManager.Load<Texture2D>("Images/player");
            SoundEffect playerSound = contentManager.Load<SoundEffect>("Audio/click");
            AddEntity(Player.Create(playerTex, playerSound, controlManager, SceneContext.Game, new Vector2(0, 0)));
            AddEntity(Wall.Create(playerTex, new Vector2(100, 100)));
        }

        override public SceneContext ProcessInput(GameTime gameTime)
        {
            return SceneContext.Game;
        }

        override public void Render(GameTime gameTime)
        {
        }

        override public void Update(GameTime gameTime)
        {
            keyboardInput.Update(gameTime);
            movement.Update(gameTime);
            collision.Update(gameTime);
            renderer.Update(gameTime);
            audio.Update(gameTime);
        }

        private void AddEntity(Entity entity)
        {
            renderer.Add(entity);
            keyboardInput.Add(entity);
            movement.Add(entity);
            collision.Add(entity);
            audio.Add(entity);
        }

        private void RemoveEntity(Entity entity)
        {
            renderer.Remove(entity.id);
            keyboardInput.Remove(entity.id);
            movement.Remove(entity.id);
            collision.Remove(entity.id);
            audio.Remove(entity.id);
        }

    }
}

