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
            linker.Update(elapsedTime);
            movement.Update(elapsedTime);
            collision.Update(elapsedTime);
            spawner.Update(elapsedTime);
            systemNetwork.update(elapsedTime, MessageQueueServer.instance.getMessages());
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
                    new Shared.Controls.ControlManager(new Shared.DataManager()), playerRect, "player");
            Shared.Entities.Entity tail = Shared.Entities.Tail.Create("Images/player", Color.White, "Audio/bass-switch",
                    new Shared.Controls.ControlManager(new Shared.DataManager()), playerRect, "player");
            Shared.Entities.Entity body1 = Shared.Entities.Body.Create("Images/player", Color.White, "Audio/bass-switch",
                    new Shared.Controls.ControlManager(new Shared.DataManager()), playerRect, "player");
            Shared.Entities.Entity body2 = Shared.Entities.Body.Create("Images/player", Color.White, "Audio/bass-switch",
                    new Shared.Controls.ControlManager(new Shared.DataManager()), playerRect, "player");
            Shared.Entities.Entity body3 = Shared.Entities.Body.Create("Images/player", Color.White, "Audio/bass-switch",
                    new Shared.Controls.ControlManager(new Shared.DataManager()), playerRect, "player");
            Shared.Entities.Entity body4 = Shared.Entities.Body.Create("Images/player", Color.White, "Audio/bass-switch",
                    new Shared.Controls.ControlManager(new Shared.DataManager()), playerRect, "player");
            Shared.Entities.Entity body5 = Shared.Entities.Body.Create("Images/player", Color.White, "Audio/bass-switch",
                    new Shared.Controls.ControlManager(new Shared.DataManager()), playerRect, "player");
            Shared.Entities.Entity body6 = Shared.Entities.Body.Create("Images/player", Color.White, "Audio/bass-switch",
                    new Shared.Controls.ControlManager(new Shared.DataManager()), playerRect, "player");
            Shared.Entities.Entity body7 = Shared.Entities.Body.Create("Images/player", Color.White, "Audio/bass-switch",
                    new Shared.Controls.ControlManager(new Shared.DataManager()), playerRect, "player");
            Shared.Entities.Entity body8 = Shared.Entities.Body.Create("Images/player", Color.White, "Audio/bass-switch",
                    new Shared.Controls.ControlManager(new Shared.DataManager()), playerRect, "player");
            Shared.Entities.Entity body9 = Shared.Entities.Body.Create("Images/player", Color.White, "Audio/bass-switch",
                    new Shared.Controls.ControlManager(new Shared.DataManager()), playerRect, "player");
            Shared.Entities.Entity body10 = Shared.Entities.Body.Create("Images/player", Color.White, "Audio/bass-switch",
                    new Shared.Controls.ControlManager(new Shared.DataManager()), playerRect, "player");
            Shared.Entities.Entity body11 = Shared.Entities.Body.Create("Images/player", Color.White, "Audio/bass-switch",
                    new Shared.Controls.ControlManager(new Shared.DataManager()), playerRect, "player");
            Shared.Entities.Entity body12 = Shared.Entities.Body.Create("Images/player", Color.White, "Audio/bass-switch",
                    new Shared.Controls.ControlManager(new Shared.DataManager()), playerRect, "player");
            Shared.Entities.Entity body13 = Shared.Entities.Body.Create("Images/player", Color.White, "Audio/bass-switch",
                    new Shared.Controls.ControlManager(new Shared.DataManager()), playerRect, "player");

            clientToEntityId[clientId] = player.id;

            MessageQueueServer.instance.sendMessage(clientId, new Shared.Messages.NewEntity(player));
            MessageQueueServer.instance.sendMessage(clientId, new Shared.Messages.NewEntity(tail));
            MessageQueueServer.instance.sendMessage(clientId, new Shared.Messages.NewEntity(body1));
            MessageQueueServer.instance.sendMessage(clientId, new Shared.Messages.NewEntity(body2));
            MessageQueueServer.instance.sendMessage(clientId, new Shared.Messages.NewEntity(body3));
            MessageQueueServer.instance.sendMessage(clientId, new Shared.Messages.NewEntity(body4));
            MessageQueueServer.instance.sendMessage(clientId, new Shared.Messages.NewEntity(body5));
            MessageQueueServer.instance.sendMessage(clientId, new Shared.Messages.NewEntity(body6));
            MessageQueueServer.instance.sendMessage(clientId, new Shared.Messages.NewEntity(body7));
            MessageQueueServer.instance.sendMessage(clientId, new Shared.Messages.NewEntity(body8));
            MessageQueueServer.instance.sendMessage(clientId, new Shared.Messages.NewEntity(body9));
            MessageQueueServer.instance.sendMessage(clientId, new Shared.Messages.NewEntity(body10));
            MessageQueueServer.instance.sendMessage(clientId, new Shared.Messages.NewEntity(body11));
            MessageQueueServer.instance.sendMessage(clientId, new Shared.Messages.NewEntity(body12));
            MessageQueueServer.instance.sendMessage(clientId, new Shared.Messages.NewEntity(body13));

            // Other clients do not need this
            // player.Remove<Shared.Components.MouseControllable>();
            // player.Remove<Shared.Components.KeyboardControllable>();
            player.Remove<Shared.Components.Camera>();

            Shared.Messages.Message message = new Shared.Messages.NewEntity(player);
            foreach (int otherId in clients)
            {
                if (otherId != clientId)
                {
                    MessageQueueServer.instance.sendMessage(otherId, message);
                }
            }
            AddEntity(player);
        }
    }
}

