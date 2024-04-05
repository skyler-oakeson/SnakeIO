using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Shared.Components;
using Shared.Entities;
using Shared.Messages;
using Microsoft.Xna.Framework.Audio;
using Shared.Controls;

namespace Server
{
    public class GameModel
    {
        public int HEIGHT { get; private set; }
        public int WIDTH { get; private set; }

        private HashSet<int> clients = new HashSet<int>();
        private Dictionary<uint, Entity> entities = new Dictionary<uint, Entity>(); // may not need
        private Dictionary<int, uint> clientToEntityId = new Dictionary<int, uint>();


        // private Systems.Renderer renderer;
        // private Systems.KeyboardInput keyboardInput;
        private Systems.Network systemNetwork;
        private Systems.Movement movement;
        private Systems.Collision collision;
        private Systems.Audio audio;
        private Systems.Spawner spawner;

        public delegate void AddDelegate(Entity entity);
        private AddDelegate addEntity;

        private List<Entity> toRemove = new List<Entity>();
        private List<Entity> toAdd = new List<Entity>();

        public GameModel()
        {
            addEntity = AddEntity;
        }

        public void Initialize(Shared.Controls.ControlManager controlManager, SpriteBatch spriteBatch, ContentManager contentManager)
        {
            //this.keyboardInput = new Systems.KeyboardInput(controlManager, Scenes.SceneContext.Game);
            this.movement = new Systems.Movement();
            //this.renderer = new Renderer(spriteBatch);
            this.collision = new Systems.Collision();
            this.audio = new Systems.Audio();
            this.spawner = new Systems.Spawner(addEntity);
            this.systemNetwork = new Systems.Network();
            this.controlManager = controlManager;

            systemNetwork.registerJoinHandler(handleJoin);
            systemNetwork.registerDisconnectHandler(handleDisconnect);
            MessageQueueServer.instance.registerConnectHandler(handleConnect);

        }

        public void Update(GameTime gameTime)
        {
            //keyboardInput.Update(gameTime);
            movement.Update(gameTime);
            collision.Update(gameTime);
            audio.Update(gameTime);
            spawner.Update(gameTime);
            systemNetwork.update(gameTime, MessageQueueServer.instance.getMessages());
        }

        public void Render(GameTime gameTime)
        {
            //renderer.Update(gameTime);
        }

        private void AddEntity(Entity entity)
        {
            //renderer.Add(entity);
            //keyboardInput.Add(entity);
            movement.Add(entity);
            collision.Add(entity);
            audio.Add(entity);
            spawner.Add(entity);
            systemNetwork.Add(entity);
            entities[entity.id] = entity;
        }

        private void RemoveEntity(Entity entity)
        {
            //renderer.Remove(entity.id);
            //keyboardInput.Remove(entity.id);
            movement.Remove(entity.id);
            collision.Remove(entity.id);
            audio.Remove(entity.id);
            spawner.Remove(entity.id);
            systemNetwork.Remove(entity.id);
            entities.Remove(entity.id);
        }

        private void RemoveEntity(uint id)
        {
            entities.Remove(id);
        }

        private void handleConnect(int clientId)
        {
            clients.Add(clientId);
            MessageQueueServer.instance.sendMessage(clientId, new Shared.Messages.ConnectAck());
        }

        private void handleDisconnect(int clientId)
        {
            clients.Remove(clientId);
            Message message = new Shared.Messages.RemoveEntity(clientToEntityId[clientId]);
            MessageQueueServer.instance.broadcastMessage(message);
            RemoveEntity(clientToEntityId[clientId]);
            clientToEntityId.Remove(clientId);
        }

        private void reportAllEntities(int clientId)
        {
            foreach (var item in entities)
            {
                MessageQueueServer.instance.sendMessage(clientId, new Shared.Messages.NewEntity(item.Value));
            }
        }

        private void handleJoin(int clientId)
        {
            reportAllEntities(clientId);

            Entity player = Shared.Entities.Player.Create(this.playerTex, this.playerSound, this.controlManager, Scenes.SceneContext.Game, new Vector2(0, 0));
            clientToEntityId[clientId] = player.id;

            MessageQueueServer.instance.sendMessage(clientId, new NewEntity(player));


            Message message = new NewEntity(player);
            foreach (int otherId in clients)
            {
                if (otherId != clientId)
                {
                    MessageQueueServer.instance.sendMessage(otherId, message);
                }
            }
        }
    }
}

