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
        private Color[] playerColors = {Color.Orange, Color.Red, Color.Blue, Color.Gold, Color.Pink, Color.Purple, Color.Green, Color.Yellow, Color.DarkRed, Color.OrangeRed};
        private Shared.MyRandom random = new Shared.MyRandom();

        private Systems.Network systemNetwork;
        private Shared.Systems.Movement movement;
        private Systems.Collision collision;
        private Systems.Spawner spawner;
        private Systems.Growth growth;
        private Systems.ParticleSystem particleSystem;
        private Shared.Systems.Linker linker;
        private Shared.Controls.ControlManager controlManager = new Shared.Controls.ControlManager(new Shared.DataManager());

        public delegate void AddDelegate(Shared.Entities.Entity entity);
        private AddDelegate addEntity;
        public delegate void RemoveDelegate(Shared.Entities.Entity entity);
        private RemoveDelegate removeEntity;

        private List<Shared.Entities.Entity> players = new List<Shared.Entities.Entity>();

        private List<Shared.Entities.Entity> toRemove = new List<Shared.Entities.Entity>();
        private List<Shared.Entities.Entity> toAdd = new List<Shared.Entities.Entity>();

        public GameModel()
        {
            addEntity = AddEntity;
            removeEntity = RemoveEntity;
        }

        public bool Initialize()
        {
            this.movement = new Shared.Systems.Movement();
            this.collision = new Systems.Collision(addEntity, removeEntity);
            this.spawner = new Systems.Spawner(addEntity);
            this.systemNetwork = new Systems.Network();
            this.linker = new Shared.Systems.Linker();
            this.growth = new Systems.Growth(addEntity);
            this.particleSystem = new Systems.ParticleSystem(addEntity, removeEntity);

            systemNetwork.registerJoinHandler(handleJoin);
            systemNetwork.registerDisconnectHandler(handleDisconnect);
            MessageQueueServer.instance.registerConnectHandler(handleConnect);
            Rectangle rectangle = new Rectangle(50000, 50000, 10, 10);
            AddEntity(Shared.Entities.Food.Create("Images/food", rectangle));
            new Utils.WorldGenerator(addEntity);

            return true;
        }

        public void Update(TimeSpan elapsedTime)
        {
            DateTime startTime = DateTime.Now;
            systemNetwork.update(elapsedTime, MessageQueueServer.instance.getMessages());
            TimeSpan currentTime = DateTime.Now - startTime;
            //Console.WriteLine($"Network update time: {currentTime}");
            startTime = DateTime.Now;
            linker.Update(elapsedTime);
            currentTime = DateTime.Now - startTime;
            //Console.WriteLine($"Linker update time: {currentTime}");
            startTime = DateTime.Now;
            movement.Update(elapsedTime);
            currentTime = DateTime.Now - startTime;
            //Console.WriteLine($"Movement update time: {currentTime}");
            startTime = DateTime.Now;
            collision.Update(elapsedTime);
            currentTime = DateTime.Now - startTime;
            particleSystem.Update(elapsedTime);
            //Console.WriteLine($"Collision update time: {currentTime}");
            startTime = DateTime.Now;
            spawner.Update(elapsedTime);
            currentTime = DateTime.Now - startTime;
            //Console.WriteLine($"Spawner update time: {currentTime}");
            startTime = DateTime.Now;
            growth.Update(elapsedTime);
            currentTime = DateTime.Now - startTime;
            //Console.WriteLine($"Growth update time: {currentTime}");
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
            growth.Add(entity);
            systemNetwork.Add(entity);
            particleSystem.Add(entity);
            entities[entity.id] = entity;
        }

        private void RemoveEntity(Shared.Entities.Entity entity)
        {
            movement.Remove(entity.id);
            collision.Remove(entity.id);
            spawner.Remove(entity.id);
            linker.Remove(entity.id);
            growth.Remove(entity.id);
            systemNetwork.Remove(entity.id);
            particleSystem.Remove(entity.id);
            entities.Remove(entity.id);
        }

        private void RemoveEntity(uint id)
        {
            movement.Remove(id);
            collision.Remove(id);
            spawner.Remove(id);
            linker.Remove(id);
            growth.Remove(id);
            systemNetwork.Remove(id);
            particleSystem.Remove(id);
            entities.Remove(id);
        }

        public void shutdown()
        {

        }

        private void handleConnect(int clientId)
        {
            Console.WriteLine("Connect");
            clients.Add(clientId);
            MessageQueueServer.instance.sendMessage(clientId, new Shared.Messages.ConnectAck());
        }

        private void handleDisconnect(int clientId)
        {
            clients.Remove(clientId);
            Shared.Messages.Message message = new Shared.Messages.RemoveEntity(clientToEntityId[clientId]);
            MessageQueueServer.instance.broadcastMessage(message);
            clientToEntityId.Remove(clientId);
        }

        private void reportAllEntities(int clientId)
        {
            foreach (Shared.Entities.Entity item in entities.Values)
            {
                MessageQueueServer.instance.sendMessage(clientId, new Shared.Messages.NewEntity(item));
            }
        }

        private void handleJoin(int clientId, Shared.Messages.Join message)
        {
            reportAllEntities(clientId);

            Console.WriteLine("HANDLE JOIN");

            Rectangle playerRect = new Rectangle(0, 0, 50, 50); //TODO: update width and height
            Shared.Entities.Entity player = Shared.Entities.Player.Create(clientId, message.name, "Images/head", Color.White, playerRect, $"{clientId}");
            MessageQueueServer.instance.sendMessage(clientId, new Shared.Messages.NewEntity(player));
            players.Add(player);

            Color color = playerColors[random.Next(0, playerColors.Count())];
            Shared.Entities.Entity body = Shared.Entities.Body.Create("Images/body", color, playerRect, $"{clientId}", Shared.Components.LinkPosition.Body);
            MessageQueueServer.instance.sendMessage(clientId, new Shared.Messages.NewEntity(body));
            AddEntity(body);
            Shared.Entities.Entity tail = Shared.Entities.Body.Create("Images/tail", Color.White, playerRect, $"{clientId}", Shared.Components.LinkPosition.Tail);
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

