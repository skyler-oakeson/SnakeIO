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
        private Systems.MouseInput mouseInput;
        private Systems.Linker linker;
        private Systems.Audio audio;

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
            this.renderer = new Systems.Renderer(spriteBatch);
            this.network = new Systems.Network();
            this.interpolation = new Systems.Interpolation();
            network.registerNewEntityHandler(handleNewEntity);
            network.registerRemoveEntityHandler(handleRemoveEntity);
            this.keyboardInput = new Systems.KeyboardInput(controlManager);
            this.mouseInput = new Systems.MouseInput(controlManager);
            this.audio = new Systems.Audio();
            this.linker = new Systems.Linker();
            this.contentManager = contentManager;

            Texture2D foodTex = contentManager.Load<Texture2D>("Images/food");
            Texture2D playerTex = contentManager.Load<Texture2D>("Images/player");
            SoundEffect playerSound = contentManager.Load<SoundEffect>("Audio/click");
        }

        public void Update(TimeSpan elapsedTime)
        {
            network.update(elapsedTime, MessageQueueClient.instance.getMessages());
            renderer.Update(elapsedTime);
            keyboardInput.Update(elapsedTime);
            mouseInput.Update(elapsedTime);
            audio.Update(elapsedTime);
            linker.Update(elapsedTime);
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
            mouseInput.Add(entity);
            audio.Add(entity);
            linker.Add(entity);

            entities[entity.id] = entity;
        }

        private void RemoveEntity(Entity entity)
        {
            renderer.Remove(entity.id);
            keyboardInput.Remove(entity.id);
            network.Remove(entity.id);
            interpolation.Remove(entity.id);
            mouseInput.Remove(entity.id);
            audio.Remove(entity.id);
            linker.Remove(entity.id);

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

            if (message.hasAppearance)
            {
                entity.Add(new Shared.Components.Appearance(message.appearanceMessage.texturePath, message.appearanceMessage.color, message.appearanceMessage.stroke));
                Shared.Components.Appearance appearance = entity.GetComponent<Shared.Components.Appearance>();
                Texture2D texture = contentManager.Load<Texture2D>(appearance.texturePath);
                entity.Add(new Shared.Components.Renderable(texture, appearance.texturePath, appearance.color, appearance.stroke));
            }

            if (message.hasPosition)
            {
                entity.Add(new Shared.Components.Positionable(new Vector2(message.positionableMessage.pos.X, message.positionableMessage.pos.Y)));
            }

            //TODO: find other ways to handle collidable. Maybe we specify what the radius is so that we don't have to calculate it.
            //There is no guaruntee that if it has position and has appearance that it will be collidable
            if (message.hasCollidable)
            {
                Shared.Components.Renderable renderable = entity.GetComponent<Shared.Components.Renderable>();
                int radius = renderable.texture.Width >= renderable.texture.Height ? renderable.texture.Width / 2 : renderable.texture.Height / 2;
                entity.Add(new Shared.Components.Collidable(new Vector3(message.positionableMessage.pos.X, message.positionableMessage.pos.Y, radius)));
            }

            if (message.hasMovement)
            {
                entity.Add(new Shared.Components.Movable(new Vector2(message.movableMessage.rotation.X, message.movableMessage.rotation.Y), new Vector2(message.movableMessage.velocity.X, message.movableMessage.velocity.Y)));
            }

            if (message.hasSpawnable)
            {
                entity.Add(new Shared.Components.Spawnable(message.spawnableMessage.spawnRate, message.spawnableMessage.spawnCount, message.spawnableMessage.type));
            }

            if (message.hasKeyboardControllable)
            {
                //Do Something
            }

            if (message.hasMouseControllable)
            {
                //Do Something
            }

            return entity;
        }


    }
}
