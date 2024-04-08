using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Shared.Entities;
using System.Diagnostics;

namespace SnakeIO
{
    public class GameModel
    {
        public int HEIGHT { get; private set; }
        public int WIDTH { get; private set; }

        private Dictionary<uint, Entity> entities = new Dictionary<uint, Entity>(); // may not need

        private Systems.Renderer renderer;
        private Systems.KeyboardInput keyboardInput;
        private Systems.Network network;
        private Systems.Interpolation interpolation;

        private ContentManager contentManager;
        private Shared.Controls.ControlManager controlManager;

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
            this.renderer = new Systems.Renderer(spriteBatch);
            this.network = new Systems.Network();
            this.interpolation = new Systems.Interpolation();
            this.controlManager = controlManager;
            this.contentManager = contentManager;
            network.registerNewEntityHandler(handleNewEntity);
            network.registerRemoveEntityHandler(handleRemoveEntity);

            Texture2D foodTex = contentManager.Load<Texture2D>("Images/food");
            Texture2D playerTex = contentManager.Load<Texture2D>("Images/player");
            SoundEffect playerSound = contentManager.Load<SoundEffect>("Audio/click");
        }

        public void Update(TimeSpan elapsedTime)
        {
            network.update(elapsedTime, MessageQueueClient.instance.getMessages());
            renderer.Update(elapsedTime);
            try
            {
                keyboardInput.Update(elapsedTime);
            }
            catch
            {
                Console.WriteLine("Skill Issue");
            }
        }

        public void Render(TimeSpan elapsedTime)
        {
            renderer.Update(elapsedTime);
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

            if (message.appearance != null)
            {
                //Texture2D texture = contentManager.Load<Texture2D>(message.texturePath);
                //entity.Add(new Shared.Components.Renderable(texture, new Color(message.color.R, message.color.G, message.color.B, message.color.A), new Color(message.stroke.R, message.stroke.G, message.stroke.B, message.stroke.A)));
            }

            if (message.positionable != null)
            {
                entity.Add(new Shared.Components.Positionable(
                            new Vector2(message.positionableMessage.pos.X, message.positionableMessage.pos.Y)));
            }

            if (message.renderable != null)
            {
                Shared.Components.Appearance appearance = entity.GetComponent<Shared.Components.Appearance>();
                Texture2D texture = contentManager.Load<Texture2D>(appearance.texturePath);
                entity.Add(new Shared.Components.Renderable(texture, appearance.color, appearance.stroke));
            }

            //TODO: find other ways to handle collidable. Maybe we specify what the radius is so that we don't have to calculate it. 
            //There is no guaruntee that if it has position and has appearance that it will be collidable
            if (message.collidable != null)
            {
                Shared.Components.Renderable renderable = entity.GetComponent<Shared.Components.Renderable>();
                int radius = renderable.Texture.Width >= renderable.Texture.Height ? renderable.Texture.Width / 2 : renderable.Texture.Height / 2;
                entity.Add(new Shared.Components.Collidable(new Vector3(message.positionableMessage.pos.X, message.positionableMessage.pos.Y, radius)));
            }

            if (message.movable != null)
            {
                entity.Add(new Shared.Components.Movable(new Vector2(message.movableMessage.rotation.X, message.movableMessage.rotation.Y), new Vector2(message.movableMessage.velocity.X, message.movableMessage.velocity.Y)));
            }

            if (message.spawnable != null)
            {
                entity.Add(new Shared.Components.Spawnable(message.spawnableMessage.spawnRate, message.spawnableMessage.spawnCount, message.spawnableMessage.type));
            }

            if (message.keyboardControllable != null)
            {
                //Do Something
            }

            if (message.mouseControllable != null)
            {
                //Do Something
            }

            return entity;
        }


    }
}
