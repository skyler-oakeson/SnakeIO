using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics;

namespace Server
{
    public class GameModel
    {
        private HashSet<int> clients = new HashSet<int>();
        private Dictionary<uint, Shared.Entities.Entity> entities = new Dictionary<uint, Shared.Entities.Entity>(); // may not need
        private Dictionary<int, uint> clientToEntityId = new Dictionary<int, uint>();

        private Systems.Network systemNetwork;
        private Shared.Systems.Movement movement;
        private Systems.Collision collision;
        private Systems.Spawner spawner;
        private Shared.Systems.Linker linker;
        private Shared.Controls.ControlManager controlManager = new Shared.Controls.ControlManager(new Shared.DataManager());

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
            this.movement = new Shared.Systems.Movement();
            this.collision = new Systems.Collision();
            this.spawner = new Systems.Spawner(addEntity);
            this.systemNetwork = new Systems.Network();
            this.linker = new Shared.Systems.Linker();

            systemNetwork.registerJoinHandler(handleJoin);
            systemNetwork.registerDisconnectHandler(handleDisconnect);
            MessageQueueServer.instance.registerConnectHandler(handleConnect);
            Rectangle rectangle = new Rectangle(100, 100, 10, 10);
            AddEntity(Shared.Entities.Food.Create("Images/food", rectangle));
            new Utils.WorldGenerator(addEntity);

            return true;
        }

        public void Update(TimeSpan elapsedTime)
        {
            systemNetwork.update(elapsedTime, MessageQueueServer.instance.getMessages());
            linker.Update(elapsedTime);
            movement.Update(elapsedTime);
            collision.Update(elapsedTime);
            spawner.Update(elapsedTime);
        }

        public void Render(GameTime gameTime)
        {
        }

        private void AddEntity(Shared.Entities.Entity entity)
        {
            movement.Add(entity);
            collision.Add(entity);
            spawner.Add(entity);
            linker.Add(entity);
            systemNetwork.Add(entity);
            entities[entity.id] = entity;
        }

        private void RemoveEntity(Shared.Entities.Entity entity)
        {
            movement.Remove(entity.id);
            collision.Remove(entity.id);
            spawner.Remove(entity.id);
            linker.Remove(entity.id);
            systemNetwork.Remove(entity.id);
            entities.Remove(entity.id);
        }

        private void RemoveEntity(uint id)
        {
            movement.Remove(id);
            collision.Remove(id);
            spawner.Remove(id);
            linker.Remove(id);
            systemNetwork.Remove(id);
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
            RemoveEntity(entities[clientToEntityId[clientId]]);
            clientToEntityId.Remove(clientId);
        }

        private void reportAllEntities(int clientId)
        {
            foreach (Shared.Entities.Entity item in entities.Values)
            {
                MessageQueueServer.instance.sendMessage(clientId, new Shared.Messages.NewEntity(item));
            }
        }

        private void handleJoin(int clientId)
        {
            reportAllEntities(clientId);

            Rectangle playerRect = new Rectangle(0, 0, 50, 50); //TODO: update width and height
            Shared.Entities.Entity player = Shared.Entities.Player.Create("Images/head", Color.Blue, "Audio/bass-switch", playerRect, $"{clientId}");
            MessageQueueServer.instance.sendMessage(clientId, new Shared.Messages.NewEntity(player));

            for (int i = 0; i < 20; i++)
            {
                Shared.Entities.Entity body = Shared.Entities.Body.Create("Images/body", Color.White, "Audio/bass-switch", playerRect, $"{clientId}", Shared.Components.LinkPosition.Body);
                MessageQueueServer.instance.sendMessage(clientId, new Shared.Messages.NewEntity(body));
                AddEntity(body);
            }

            Shared.Entities.Entity tail = Shared.Entities.Body.Create("Images/tail", Color.Red, "Audio/bass-switch", playerRect, $"{clientId}", Shared.Components.LinkPosition.Tail);
            MessageQueueServer.instance.sendMessage(clientId, new Shared.Messages.NewEntity(tail));
            AddEntity(tail);

            clientToEntityId[clientId] = player.id;

            // Other clients do not need this
            player.Remove<Shared.Components.Camera>();
            player.Remove<Shared.Components.KeyboardControllable>();

            foreach (int otherId in clients)
            {
                if (otherId != clientId)
                {
                    MessageQueueServer.instance.sendMessage(otherId, new Shared.Messages.NewEntity(player));
                }
            }

            AddEntity(player);
        }
    }
}

