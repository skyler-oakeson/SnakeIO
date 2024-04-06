using Systems;
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

        private Renderer renderer;
        private KeyboardInput keyboardInput;
        private Network network;
        private Interpolation interpolation;

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
            this.renderer = new Renderer(spriteBatch);
            this.network = new Network();
            this.interpolation = new Interpolation();
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

            if (message.hasAppearance)
            {
                Texture2D texture = contentManager.Load<Texture2D>(message.texturePath);
                entity.Add(new Shared.Components.Renderable(texture, new Color(message.color.R, message.color.G, message.color.B), new Color(message.stroke.R, message.stroke.G, message.stroke.B)));
            }

            if (message.hasPosition)
            {
                entity.Add(new Shared.Components.Positionable(new Vector2(message.position.X, message.position.Y)));
            }

            //TODO: find other ways to handle collidable. Maybe we specify what the radius is so that we don't have to calculate it. 
            //There is no guaruntee that if it has position and has appearance that it will be collidable
            if (message.hasPosition && message.hasAppearance)
            {
                Shared.Components.Renderable renderable = entity.GetComponent<Shared.Components.Renderable>();
                int radius = renderable.Texture.Width >= renderable.Texture.Height ? renderable.Texture.Width / 2 : renderable.Texture.Height / 2;
                entity.Add(new Shared.Components.Collidable(new Vector3(message.position.X, message.position.Y, radius)));
            }

            if (message.hasMovement)
            {
                entity.Add(new Shared.Components.Movable(new Vector2(message.rotation.X, message.rotation.Y), new Vector2(message.velocity.X, message.velocity.Y)));
            }

            if (message.hasSpawnable)
            {
                entity.Add(new Shared.Components.Spawnable(message.spawnRate, message.spawnCount, message.spawnType));
            }

            //TODO: update NewEntity message and here to hold all needed components

            //TODO: do input
            if (message.hasInput)
            {
                foreach (var input in message.inputs)
                {
                    entity.Add(new Shared.Components.KeyboardControllable(
                                controlManager,
                                new (Shared.Controls.Control, Shared.Controls.ControlDelegate)[4]
                                {
                                (new Shared.Controls.Control(input.sc, input.cc, input.key, null, input.keyPressOnly),
                                 new Shared.Controls.ControlDelegate((TimeSpan elapsedTime, float value) =>
                                     {
                                     entity.GetComponent<Shared.Components.Movable>().Velocity += new Vector2(0, -.2f);
                                     })),
                                (new Shared.Controls.Control(input.sc, input.cc, input.key, null, input.keyPressOnly),
                                 new Shared.Controls.ControlDelegate((TimeSpan elapsedTime, float value) =>
                                     {
                                     entity.GetComponent<Shared.Components.Movable>().Velocity += new Vector2(0, .2f);
                                     })),
                                (new Shared.Controls.Control(input.sc, input.cc, input.key, null, input.keyPressOnly),
                                 new Shared.Controls.ControlDelegate((TimeSpan elapsedTime, float value) =>
                                     {
                                     entity.GetComponent<Shared.Components.Movable>().Velocity += new Vector2(-.2f, 0);
                                     })),
                                (new Shared.Controls.Control(input.sc, input.cc, input.key, null, input.keyPressOnly),
                                 new Shared.Controls.ControlDelegate((TimeSpan elapsedTime, float value) =>
                                     {
                                     entity.GetComponent<Shared.Components.Movable>().Velocity += new Vector2(.2f, 0);
                                     }))
                                }
                    ));
                }
            }

            return entity;
        }


    }
}
