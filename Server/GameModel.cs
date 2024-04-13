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


        private Systems.InputHandler inputHandler;
        private Systems.Network systemNetwork;
        private Systems.Movement movement;
        private Systems.Collision collision;
        private Systems.Spawner spawner;
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
            this.inputHandler = new Systems.InputHandler(controlManager);
            this.movement = new Systems.Movement();
            this.collision = new Systems.Collision();
            this.spawner = new Systems.Spawner(addEntity);
            this.systemNetwork = new Systems.Network();

            systemNetwork.registerJoinHandler(handleJoin);
            systemNetwork.registerDisconnectHandler(handleDisconnect);
            MessageQueueServer.instance.registerConnectHandler(handleConnect);
            Rectangle rectangle = new Rectangle(100, 100, 10, 10);
            AddEntity(Shared.Entities.Food.Create("Images/food", rectangle));

            return true;
        }

        public void Update(TimeSpan elapsedTime)
        {
            //keyboardInput.Update(gameTime);
            movement.Update(elapsedTime);
            collision.Update(elapsedTime);
            // spawner.Update(elapsedTime);
            systemNetwork.update(elapsedTime, MessageQueueServer.instance.getMessages());
        }

        public void Render(GameTime gameTime)
        {
        }

        private void AddEntity(Shared.Entities.Entity entity)
        {
            inputHandler.Add(entity);
            movement.Add(entity);
            collision.Add(entity);
            spawner.Add(entity);
            systemNetwork.Add(entity);
            entities[entity.id] = entity;
        }

        private void RemoveEntity(Shared.Entities.Entity entity)
        {
            inputHandler.Remove(entity.id);
            movement.Remove(entity.id);
            collision.Remove(entity.id);
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

            Rectangle playerRect = new Rectangle(0, 0, 50, 50); //TODO: update width and height
            Shared.Entities.Entity player = Shared.Entities.Player.Create("Images/player", Color.White, "Audio/bass-switch",
                    new Shared.Controls.ControlManager(new Shared.DataManager()), playerRect);
            clientToEntityId[clientId] = player.id;
            Rectangle foodRect = new Rectangle(200, 200, 10, 10);
            Shared.Entities.Entity food = Shared.Entities.Food.Create("Images/food", foodRect);
            clientToEntityId[clientId] = food.id;

            MessageQueueServer.instance.sendMessage(clientId, new Shared.Messages.NewEntity(player));
            MessageQueueServer.instance.sendMessage(clientId, new Shared.Messages.NewEntity(food));

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

