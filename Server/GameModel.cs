using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics;

namespace Server
{
    public class GameModel
    {
        public int HEIGHT { get; private set; }
        public int WIDTH { get; private set; }

        private HashSet<int> clients = new HashSet<int>();
        private Dictionary<uint, Shared.Entities.Entity> entities = new Dictionary<uint, Shared.Entities.Entity>(); // may not need
        private Dictionary<int, uint> clientToEntityId = new Dictionary<int, uint>();


        // private Systems.KeyboardInput keyboardInput;
        private Systems.Network systemNetwork;
        private Systems.Movement movement;
        private Systems.Collision collision;
        private Systems.Audio audio;
        private Systems.Spawner spawner;

        public delegate void AddDelegate(Shared.Entities.Entity entity);
        private AddDelegate addEntity;

        private List<Shared.Entities.Entity> toRemove = new List<Shared.Entities.Entity>();
        private List<Shared.Entities.Entity> toAdd = new List<Shared.Entities.Entity>();

        public GameModel()
        {
            addEntity = AddEntity;
        }

        public bool Initialize()
        {
            //this.keyboardInput = new Systems.KeyboardInput(controlManager, Scenes.SceneContext.Game);
            this.movement = new Systems.Movement();
            this.collision = new Systems.Collision();
            this.audio = new Systems.Audio();
            this.spawner = new Systems.Spawner(addEntity);
            this.systemNetwork = new Systems.Network();

            systemNetwork.registerJoinHandler(handleJoin);
            systemNetwork.registerDisconnectHandler(handleDisconnect);
            MessageQueueServer.instance.registerConnectHandler(handleConnect);

            return true;
        }

        public void Update(TimeSpan elapsedTime)
        {
            //keyboardInput.Update(gameTime);
            movement.Update(elapsedTime);
            collision.Update(elapsedTime);
            audio.Update(elapsedTime);
            spawner.Update(elapsedTime);
            systemNetwork.update(elapsedTime, MessageQueueServer.instance.getMessages());
        }

        public void Render(GameTime gameTime)
        {
        }

        private void AddEntity(Shared.Entities.Entity entity)
        {
            //keyboardInput.Add(entity);
            movement.Add(entity);
            collision.Add(entity);
            audio.Add(entity);
            spawner.Add(entity);
            systemNetwork.Add(entity);
            entities[entity.id] = entity;
        }

        private void RemoveEntity(Shared.Entities.Entity entity)
        {
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

        public void shutdown()
        {

        }

        private void handleConnect(int clientId)
        {
            clients.Add(clientId);
            MessageQueueServer.instance.sendMessage(clientId, new Shared.Messages.ConnectAck());
        }

        private void handleDisconnect(int clientId)
        {
            clients.Remove(clientId);
            Shared.Messages.Message message = new Shared.Messages.RemoveEntity(clientToEntityId[clientId]);
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

            Shared.Entities.Entity player = Shared.Entities.Player.Create("Images/player", "Audio/bass-switch", new Shared.Controls.ControlManager(new Shared.DataManager()), Scenes.SceneContext.Game, new Vector2(0, 0));
            clientToEntityId[clientId] = player.id;

            MessageQueueServer.instance.sendMessage(clientId, new Shared.Messages.NewEntity(player));

            Shared.Messages.Message message = new Shared.Messages.NewEntity(player);
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

