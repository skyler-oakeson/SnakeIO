using Systems;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Entities;

namespace SnakeIO
{
    public class GameModel
    {
        public int HEIGHT { get; private set; }
        public int WIDTH { get; private set; }

        private Renderer renderer;
        private KeyboardInput keyboardInput;
        private Movement movement;
        private Collision collision;
        private Audio audio;

        private List<Entity> toRemove = new List<Entity>();
        private List<Entity> toAdd = new List<Entity>();

        public GameModel(int height, int width)
        {
            this.HEIGHT = height;
            this.WIDTH = width;
        }

        public void Initialize(Controls.ControlManager controlManager, SpriteBatch spriteBatch, ContentManager contentManager)
        {
            this.keyboardInput = new Systems.KeyboardInput(controlManager, Scenes.SceneContext.Game);
            this.movement = new Movement();
            this.renderer = new Renderer(spriteBatch);
            this.collision = new Collision();
            this.audio = new Audio();

            Texture2D playerTex = contentManager.Load<Texture2D>("Images/player");
            SoundEffect playerSound = contentManager.Load<SoundEffect>("Audio/click");
            AddEntity(Player.Create(playerTex, playerSound, controlManager, Scenes.SceneContext.Game, new Vector2(0, 0)));
            AddEntity(Wall.Create(playerTex, new Vector2(100, 100)));
        }

        public void Update(GameTime gameTime)
        {
            keyboardInput.Update(gameTime);
            movement.Update(gameTime);
            collision.Update(gameTime);
            audio.Update(gameTime);
        }

        public void Render(GameTime gameTime)
        {
            renderer.Update(gameTime);
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
