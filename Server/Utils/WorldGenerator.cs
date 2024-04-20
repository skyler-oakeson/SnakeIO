using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Utils
{
    public class WorldGenerator
    {

        private List<Shared.Entities.Entity> worldEntities = new List<Shared.Entities.Entity>();
        private const int TILE_SIZE = 750;
        private const int WALL_WIDTH = 7500;
        private const int WALL_HEIGHT = 15;
        private const int WORLD_SIZE = 1;
        private const int WORLD_ORIGIN = 0;
        private const int WORLD_END = 7500;
        private const int TILE_COUNT = WORLD_END / TILE_SIZE;
        private Server.GameModel.AddDelegate addEntity;

        public WorldGenerator(Server.GameModel.AddDelegate addEntity)
        {
            this.addEntity = addEntity;
            this.GenerateWorld();
            foreach (var entity in worldEntities)
            {
                addEntity(entity);
            }
        }

        private void GenerateWorld()
        {
            for (int i = 0; i < TILE_COUNT; i++)
            {
                for (int j = 0; j < TILE_COUNT; j++)
                {
                    if (i == 0)
                    {
                        int x = WORLD_END / 2 - TILE_SIZE / 2;
                        int y = -TILE_SIZE / 2 - (WALL_HEIGHT / 2);
                        Shared.Entities.Entity wall = Shared.Entities.Wall.Create("Images/square", Color.White, new Rectangle(x, y, WALL_HEIGHT, WALL_WIDTH));
                        worldEntities.Add(wall);
                        Server.MessageQueueServer.instance.broadcastMessage(new Shared.Messages.NewEntity(wall));
                    }
                    if (i == TILE_COUNT - 1)
                    {
                        int x = WORLD_END / 2 - TILE_SIZE / 2;
                        int y = WORLD_END - TILE_SIZE / 2 + (WALL_HEIGHT / 2);
                        Shared.Entities.Entity wall = Shared.Entities.Wall.Create("Images/square", Color.White, new Rectangle(x, y, WALL_HEIGHT, WALL_WIDTH));
                        worldEntities.Add(wall);
                        Server.MessageQueueServer.instance.broadcastMessage(new Shared.Messages.NewEntity(wall));
                    }
                    if (j == 0)
                    {
                        int x = -TILE_SIZE / 2 - (WALL_HEIGHT / 2);
                        int y = WORLD_END / 2 - TILE_SIZE / 2;
                        Shared.Entities.Entity wall = Shared.Entities.Wall.Create("Images/square", Color.White, new Rectangle(x, y, WALL_WIDTH, WALL_HEIGHT));
                        worldEntities.Add(wall);
                        Server.MessageQueueServer.instance.broadcastMessage(new Shared.Messages.NewEntity(wall));
                    }
                    if (j == TILE_COUNT - 1)
                    {
                        int x = WORLD_END + -TILE_SIZE / 2 + (WALL_HEIGHT / 2);
                        int y = WORLD_END / 2 - TILE_SIZE / 2;
                        Shared.Entities.Entity wall = Shared.Entities.Wall.Create("Images/square", Color.White, new Rectangle(x, y, WALL_WIDTH, WALL_HEIGHT));
                        worldEntities.Add(wall);
                        Server.MessageQueueServer.instance.broadcastMessage(new Shared.Messages.NewEntity(wall));
                    }
                    if ((i != 0 || i != TILE_COUNT - 1) && (j != 0 || j != TILE_COUNT - 1))
                    {
                        Shared.Entities.Entity tile = Shared.Entities.Tile.Create("Images/Tile1", new Rectangle(j * TILE_SIZE, i * TILE_SIZE, TILE_SIZE, TILE_SIZE), Color.White);
                        worldEntities.Add(tile);
                        Server.MessageQueueServer.instance.broadcastMessage(new Shared.Messages.NewEntity(tile));
                    }
                }
            }
            //Add corner pieces
            AddCorners();

        }
        private void AddCorners()
        {
            // TOP LEFT
            int x = -TILE_SIZE / 2;
            int y = -TILE_SIZE / 2;
            Shared.Entities.Entity topLeft = Shared.Entities.Wall.Create("Images/square", Color.White, new Rectangle(x, y, WALL_HEIGHT, WALL_HEIGHT));
            worldEntities.Add(topLeft);
            Server.MessageQueueServer.instance.broadcastMessage(new Shared.Messages.NewEntity(topLeft));
            // TOP RIGHT
            x = WORLD_END - TILE_SIZE / 2;
            y = -TILE_SIZE / 2;
            Shared.Entities.Entity topRight = Shared.Entities.Wall.Create("Images/square", Color.White, new Rectangle(x, y, WALL_HEIGHT, WALL_HEIGHT));
            worldEntities.Add(topRight);
            Server.MessageQueueServer.instance.broadcastMessage(new Shared.Messages.NewEntity(topRight));
            // BOTTOM LEFT
            x = -TILE_SIZE / 2;
            y = WORLD_END - TILE_SIZE / 2;
            Shared.Entities.Entity bottomLeft = Shared.Entities.Wall.Create("Images/square", Color.White, new Rectangle(x, y, WALL_HEIGHT, WALL_HEIGHT));
            worldEntities.Add(bottomLeft);
            Server.MessageQueueServer.instance.broadcastMessage(new Shared.Messages.NewEntity(bottomLeft));
            // BOTTOM RIGHT
            x = WORLD_END - TILE_SIZE / 2;
            y = WORLD_END - TILE_SIZE / 2;
            Shared.Entities.Entity bottomRight = Shared.Entities.Wall.Create("Images/square", Color.White, new Rectangle(x, y, WALL_HEIGHT, WALL_HEIGHT));
            worldEntities.Add(bottomRight);
            Server.MessageQueueServer.instance.broadcastMessage(new Shared.Messages.NewEntity(bottomRight));
        }
    }
}
