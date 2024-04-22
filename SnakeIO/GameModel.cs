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
        private Shared.Systems.Linker linker;
        private Shared.Systems.Movement movement;
        private Systems.Audio audio;
        private Systems.Collision collision;

        private ContentManager contentManager;
        private Shared.Controls.ControlManager controlManager;
        private Shared.Entities.Entity clientPlayer;

        private List<Entity> toRemove = new List<Entity>();
        private List<Entity> toAdd = new List<Entity>();

        public GameModel(int height, int width)
        {
            this.HEIGHT = height;
            this.WIDTH = width;
        }

        public void Initialize(Shared.Controls.ControlManager controlManager, SpriteBatch spriteBatch, ContentManager contentManager)
        {
            this.renderer = new Systems.Renderer(spriteBatch);
            this.network = new Systems.Network();
            this.interpolation = new Systems.Interpolation();
            this.movement = new Shared.Systems.Movement();
            network.registerNewEntityHandler(handleNewEntity);
            network.registerRemoveEntityHandler(handleRemoveEntity);
            network.registerGameOverHandler(HandleGameOver);
            network.registerCollisionHandler(HandleCollision);
            this.keyboardInput = new Systems.KeyboardInput(controlManager);
            this.mouseInput = new Systems.MouseInput(controlManager);
            this.audio = new Systems.Audio();
            this.linker = new Shared.Systems.Linker();
            this.collision = new Systems.Collision();
            this.contentManager = contentManager;
        }

        public void Update(TimeSpan elapsedTime)
        {
            network.update(elapsedTime, MessageQueueClient.instance.getMessages());
            collision.Update(elapsedTime);
            keyboardInput.Update(elapsedTime);
            mouseInput.Update(elapsedTime);
            linker.Update(elapsedTime);
            movement.Update(elapsedTime);
            interpolation.Update(elapsedTime);
            audio.Update(elapsedTime);
        }

        public void Render(TimeSpan elapsedTime)
        {
            renderer.Update(elapsedTime);
        }

        private void AddEntity(Entity entity)
        {
            renderer.Add(entity);
            movement.Add(entity);
            keyboardInput.Add(entity);
            network.Add(entity);
            interpolation.Add(entity);
            mouseInput.Add(entity);
            audio.Add(entity);
            linker.Add(entity);
            collision.Add(entity);

            entities[entity.id] = entity;
        }

        private void RemoveEntity(Entity entity)
        {
            renderer.Remove(entity.id);
            movement.Remove(entity.id);
            keyboardInput.Remove(entity.id);
            network.Remove(entity.id);
            interpolation.Remove(entity.id);
            mouseInput.Remove(entity.id);
            audio.Remove(entity.id);
            linker.Remove(entity.id);
            collision.Remove(entity.id);

            entities.Remove(entity.id);
        }

        private void RemoveEntity(uint id)
        {
            renderer.Remove(id);
            keyboardInput.Remove(id);
            network.Remove(id);
            interpolation.Remove(id);
            collision.Remove(id);

            entities.Remove(id);
        }

        private void handleNewEntity(Shared.Messages.NewEntity message)
        {
            Entity entity = createEntity(message);
            AddEntity(entity);
        }

        private void handleRemoveEntity(Shared.Messages.RemoveEntity message)
        {
            RemoveEntity(message.id);
        }

        private void HandleGameOver(Shared.Messages.GameOver message)
        {
            //TODO: handle game over
            Console.WriteLine("Game over fool");
        }

        private void HandleCollision(Shared.Messages.Collision message)
        {
            Console.WriteLine("Collision occurred man!!!");
            Console.WriteLine(message.e1HasSound);

            if (message.e1HasSound)
            {
                contentManager.Load<SoundEffect>(message.e1SoundMessage.soundPath).Play();
            }
            else if (message.e2HasSound)
            {
                contentManager.Load<SoundEffect>(message.e2SoundMessage.soundPath).Play();
            }
        }

        private Entity createEntity(Shared.Messages.NewEntity message)
        {
            Entity entity = new Entity(message.id);

            if (message.hasSnakeID)
            {
                entity.Add(new Shared.Components.SnakeID(message.snakeIDMessage.id));
            }

            if (message.hasAppearance)
            {
                Rectangle rectangle = new Rectangle(
                        message.appearanceMessage.rectangle.X,
                        message.appearanceMessage.rectangle.Y,
                        message.appearanceMessage.rectangle.Width,
                        message.appearanceMessage.rectangle.Height);
                entity.Add(new Shared.Components.Appearance(
                            message.appearanceMessage.texturePath,
                            message.appearanceMessage.type,
                            message.appearanceMessage.color,
                            message.appearanceMessage.stroke,
                            rectangle
                            ));
                Shared.Components.Appearance appearance = entity.GetComponent<Shared.Components.Appearance>();
                Texture2D texture = contentManager.Load<Texture2D>(appearance.texturePath);
                if (appearance.type == typeof(Texture2D))
                {
                    if (message.hasAnimatable)
                    {
                        entity.Add(new Shared.Components.Animatable(message.animatableMessage.spriteTime, texture));
                    }
                    entity.Add(new Shared.Components.Renderable(texture, appearance.texturePath, appearance.color, appearance.stroke, rectangle));
                }
                else
                {
                    entity.Add(new Shared.Components.Renderable(texture, appearance.texturePath, appearance.color, appearance.stroke, rectangle));
                }
            }

            if (message.hasPosition)
            {
                entity.Add(new Shared.Components.Positionable(new Vector2(message.positionableMessage.pos.X, message.positionableMessage.pos.Y), message.positionableMessage.orientation));
            }

            if (message.hasCollidable)
            {
                entity.Add(new Shared.Components.Collidable(message.collidableMessage.Shape, message.collidableMessage.RectangleData, message.collidableMessage.CircleData));
            }

            if (message.hasMovement)
            {
                entity.Add(new Shared.Components.Movable(new Vector2(message.movableMessage.velocity.X, message.movableMessage.velocity.Y)));
            }

            if (message.hasSpawnable)
            {
                entity.Add(new Shared.Components.Spawnable(message.spawnableMessage.spawnRate, message.spawnableMessage.spawnCount, message.spawnableMessage.type));
            }

            if (message.hasConsumable)
            {
                entity.Add(new Shared.Components.Consumable(message.consumableMessage.growth));
            }

            if (message.hasGrowable)
            {
                entity.Add(new Shared.Components.Growable());
            }

            if (message.hasSound)
            {
                SoundEffect sound = contentManager.Load<SoundEffect>(message.soundMessage.soundPath);
                entity.Add(new Shared.Components.Audible(sound));
            }

            if (message.hasCamera)
            {
                Shared.Components.Positionable position = entity.GetComponent<Shared.Components.Positionable>();
                entity.Add(new Shared.Components.Camera(new Rectangle(message.cameraMessage.rectangle.X, message.cameraMessage.rectangle.Y, WIDTH, HEIGHT)));
                Shared.Components.Camera camera = entity.GetComponent<Shared.Components.Camera>();
            }

            if (message.hasLinkable)
            {
                entity.Add(new Shared.Components.Linkable(
                            message.linkableMessage.chain,
                            message.linkableMessage.linkPos
                            ));
                if (message.linkableMessage.linkPos != Shared.Components.LinkPosition.Head && message.hasPosition && message.hasMovement)
                {
                    entity.GetComponent<Shared.Components.Linkable>().linkDelegate = Shared.Entities.Body.BodyLinking;
                }
            }

            if (message.hasKeyboardControllable)
            {
                Shared.Components.Movable movable = entity.GetComponent<Shared.Components.Movable>();
                entity.Add(new Shared.Components.KeyboardControllable(message.keyboardControllableMessage.enable, message.keyboardControllableMessage.type, Shared.Entities.Player.PlayerKeyboardControls));
            }

            if (message.hasMouseControllable)
            {
                //Do Something
            }

            // This is the client's player
            if (message.hasKeyboardControllable || message.hasMouseControllable || message.hasCamera)
            {
                this.clientPlayer = entity;
            }

            return entity;
        }
    }
}
