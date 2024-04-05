using Systems;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Shared.Entities;

namespace SnakeIO
{
    public class GameModel
    {
        public int HEIGHT { get; private set; }
        public int WIDTH { get; private set; }

        private Dictionary<uint, Entity> entities = new Dictionary<uint, Entity>(); // may not need

        private Renderer renderer;
        private KeyboardInput keyboardInput;
        private Network network;
        private Interpolation interpolation;

        private ContentManager contentManager;

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

        public void Initialize(Shared.Controls.ControlManager controlManager, SpriteBatch spriteBatch, ContentManager contentManager)
        {
            this.keyboardInput = new Systems.KeyboardInput(controlManager, Scenes.SceneContext.Game);
            this.renderer = new Renderer(spriteBatch);
            this.network = new Network();
            this.interpolation = new Interpolation();
            this.contentManager = contentManager;
            network.registerNewEntityHandler(handleNewEntity);
            network.registerRemoveEntityHandler(handleRemoveEntity);

            Texture2D foodTex = contentManager.Load<Texture2D>("Images/food");
            Texture2D playerTex = contentManager.Load<Texture2D>("Images/player");
            SoundEffect playerSound = contentManager.Load<SoundEffect>("Audio/click");
            AddEntity(Player.Create(playerTex, playerSound, controlManager, Scenes.SceneContext.Game, new Vector2(0, 0)));
            AddEntity(Wall.Create(playerTex, new Vector2(100, 100)));
            AddEntity(Wall.Create(playerTex, new Vector2(200, 100)));
            AddEntity(Food.Create(foodTex, new Vector2(200, 200)));
        }

        public void Update(GameTime gameTime)
        {
            keyboardInput.Update(gameTime);
        }

        public void Render(GameTime gameTime)
        {
            renderer.Update(gameTime);
        }

        private void AddEntity(Entity entity)
        {
            renderer.Add(entity);
            keyboardInput.Add(entity);
            network.Add(entity);
            interpolation.Add(entity);

            entities[entity.id] = entity;
        }

        private void RemoveEntity(Entity entity)
        {
            renderer.Remove(entity.id);
            keyboardInput.Remove(entity.id);
            network.Remove(entity.id);
            interpolation.Remove(entity.id);
            entities.Remove(entity.id);
        }

        private void RemoveEntity(uint id)
        {
            renderer.Remove(id);
            keyboardInput.Remove(id);
            network.Remove(id);
            interpolation.Remove(id);
            entities.Remove(id);
        }

        private void handleNewEntity(Shared.Messages.NewEntity message)
        {
            Entity entity = createEntity(message);
            AddEntity(entity);
        }

        /// <summary>
        /// Handler for the RemoveEntity message.  It removes the entity from
        /// the client game model (that's us!).
        /// </summary>
        private void handleRemoveEntity(Shared.Messages.RemoveEntity message)
        {
            RemoveEntity(message.id);
        }

        private Entity createEntity(Shared.Messages.NewEntity message)
        {
            Entity entity = new Entity(message.id);

            if (message.hasRenderable)
            {
                Texture2D texture = contentManager.Load<Texture2D>(message.texture);
                entity.Add(new Shared.Components.Renderable(texture, message.color, message.color));
            }

            if (message.hasPosition)
            {
                entity.Add(new Shared.Components.Positionable(new Vector2(message.position.X, message.position.Y)));
            }

            if (message.hasMovement)
            {
            }

            if (message.hasInput)
            {
            }

            return entity;
        }


    }
}
