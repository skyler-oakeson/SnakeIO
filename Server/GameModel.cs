
using Microsoft.Xna.Framework;
using Shared.Components;
using Shared.Entities;
using Shared.Messages;

namespace Server
{
    public class GameModel
    {
        public int HEIGHT { get; private set; }
        public int WIDTH { get; private set; }

        private HashSet<int> clients = new HashSet<int>();
        //private Dictionary<uint, Entity> entities = new Dictionary<uint, Entity>();
        private Dictionary<int, uint> clientToEntityId = new Dictionary<int, uint>();

        private Network systemNetwork;

        private Renderer renderer;
        private KeyboardInput keyboardInput;
        private Movement movement;
        private Collision collision;
        private Audio audio;
        private Spawner spawner;

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

        public void Initialize(Controls.ControlManager controlManager, SpriteBatch spriteBatch, ContentManager contentManager)
        {
            this.keyboardInput = new Systems.KeyboardInput(controlManager, Scenes.SceneContext.Game);
            this.movement = new Movement();
            this.renderer = new Renderer(spriteBatch);
            this.collision = new Collision();
            this.audio = new Audio();
            this.spawner = new Spawner(addEntity);
            this.systemNetwork = new Network();

            systemNetwork.registerJoinHandler(handleJoin);
            systemNetwork.registerDisconnectHandler(handleDisconnect);
            MessageQueueServer.instance.registerConnectHandler(handleConnect);

            Texture2D foodTex = contentManager.Load<Texture2D>("Images/food");
            Texture2D playerTex = contentManager.Load<Texture2D>("Images/player");
            SoundEffect playerSound = contentManager.Load<SoundEffect>("Audio/click");
            AddEntity(Player.Create(playerTex, playerSound, controlManager, Scenes.SceneContext.Game, new Vector2(0, 0)));
            AddEntity(Wall.Create(playerTex, new Vector2(100, 100)));
            AddEntity(Wall.Create(playerTex, new Vector2(200, 100)));
            AddEntity(Food.Create(foodTex, new Vector2(200, 200)));
        }

        public void Update(GameTime gameTime)
        {
            keyboardInput.Update(gameTime);
            movement.Update(gameTime);
            collision.Update(gameTime);
            audio.Update(gameTime);
            spawner.Update(gameTime);
            systemNetwork.update(elapsedTime, MessageQueueServer.instance.getMessages());
        }

        public void Render(GameTime gameTime)
        {
            renderer.Update(gameTime);
        }

        private void AddEntity(Entity entity)
        {
            renderer.Add(entity);
            keyboardInput.Add(entity);
            movement.Add(entity);
            collision.Add(entity);
            audio.Add(entity);
            spawner.Add(entity);
            systemNetwork.Add(entity);
        }

        private void RemoveEntity(Entity entity)
        {
            renderer.Remove(entity.id);
            keyboardInput.Remove(entity.id);
            movement.Remove(entity.id);
            collision.Remove(entity.id);
            audio.Remove(entity.id);
            spawner.Remove(entity.id);
            systemNetwork.Remove(entity.id);
        }

        private void handleConnect(int clientId)
        {
            clients.Add(clientId);
            MessageQueueServer.instance.sendMessage(clientId, new Shared.Messages.ConnectAck());
        }

        private void handleDisconnect(int clientId)
        {
            clients.Remove(clientId);
            Message message = new Shared.Messages.RemoveEntity(m_clientToEntityId[clientId]);
            MessageQueueServer.instance.broadcastMessage(message);
            removeEntity(clientToEntityId[clientId]);
            clientToEntityId.Remove(clientId);
        }

        private void reportAllEntities(int clientId)
        {
            foreach (var item in m_entities)
            {
                MessageQueueServer.instance.sendMessage(clientId, new Shared.Messages.NewEntity(item.Value));
            }
        }

        private void handleJoin(int clientId)
        {
            reportAllEntities(clientId);

            Entity player = Shared.Entities.Player.create("Textures/playerShip1_blue", new Vector2(100, 100), 50, 0.1f, (float)Math.PI / 1000);
            AddEntity(player);
            m_clientToEntityId[clientId] = player.id;

            MessageQueueServer.instance.sendMessage(clientId, new NewEntity(player));

            Message message = new NewEntity(player);
            foreach (int otherId in m_clients)
            {
                if (otherId != clientId)
                {
                    MessageQueueServer.instance.sendMessage(otherId, message);
                }
            }
        }
    }
}

