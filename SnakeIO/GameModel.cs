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

        private Renderer<Texture2D> renderer;
        private KeyboardInput keyboardInput;
        private MouseInput mouseInput;
        private Movement movement;
        private Collision collision;
        private Audio audio;
        private Spawner spawner;
        private Linker linker;

        public delegate void AddDelegate(Entity entity);
        private AddDelegate addEntity;

        private List<Entity> toRemove = new List<Entity>();
        private List<Entity> toAdd = new List<Entity>();

        public GameModel(int height, int width)
        {
            this.HEIGHT = height;
            this.WIDTH = width;
            addEntity = AddEntity;
        }

        public void Initialize(Controls.ControlManager controlManager, SpriteBatch spriteBatch, ContentManager contentManager)
        {
            this.keyboardInput = new Systems.KeyboardInput(controlManager);
            this.mouseInput = new Systems.MouseInput(controlManager);
            this.movement = new Movement();
            this.renderer = new Renderer<Texture2D>(spriteBatch);
            this.collision = new Collision();
            this.audio = new Audio();
            this.spawner = new Spawner(addEntity);
            this.linker = new Linker();
            
            Texture2D foodTex = contentManager.Load<Texture2D>("Images/food");
            Texture2D playerTex = contentManager.Load<Texture2D>("Images/player");
            SoundEffect playerSound = contentManager.Load<SoundEffect>("Audio/click");

            AddEntity(Player.Create(playerTex, "1", Color.Red, playerSound, controlManager, new Vector2(100, 100)));
            AddEntity(Tail.Create(playerTex,"1", new Vector2(800, 800), Color.OrangeRed));
            AddEntity(Wall.Create(playerTex,"1", new Vector2(100, 100), Color.Blue));
            AddEntity(Wall.Create(playerTex,"1", new Vector2(100, 100), Color.Blue));
            AddEntity(Wall.Create(playerTex,"1", new Vector2(100, 100), Color.Blue));
            AddEntity(Wall.Create(playerTex,"1", new Vector2(100, 100), Color.Blue));
            AddEntity(Food.Create(foodTex, new Vector2(200, 200)));
        }

        public void Update(GameTime gameTime)
        {
            keyboardInput.Update(gameTime);
            mouseInput.Update(gameTime);
            movement.Update(gameTime);
            collision.Update(gameTime);
            audio.Update(gameTime);
            linker.Update(gameTime);
            spawner.Update(gameTime);
        }

        public void Render(GameTime gameTime)
        {
            renderer.Update(gameTime);
        }

        private void AddEntity(Entity entity)
        {
            renderer.Add(entity);
            keyboardInput.Add(entity);
            mouseInput.Add(entity);
            movement.Add(entity);
            collision.Add(entity);
            audio.Add(entity);
            linker.Add(entity);
            spawner.Add(entity);
        }

        private void RemoveEntity(Entity entity)
        {
            renderer.Remove(entity.id);
            keyboardInput.Remove(entity.id);
            mouseInput.Remove(entity.id);
            movement.Remove(entity.id);
            collision.Remove(entity.id);
            audio.Remove(entity.id);
            linker.Remove(entity.id);
            spawner.Remove(entity.id);
        }
    }
}
