using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Utils
{
    public class WorldGenerator
    {

        private List<Shared.Entities.Entity> worldEntities = new List<Shared.Entities.Entity>();
        private const int TILE_SIZE = 750;
        private const int WALL_WIDTH = 50;
        private const int WALL_HEIGHT = 50;
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
                        int wallCount = TILE_SIZE / WALL_WIDTH;
                        // For each tile, we need to render this many walls
                        for (int w = 0; w < wallCount; w++)
                        {
                            int x = ((j * (TILE_SIZE)) + (w * WALL_WIDTH)) - (TILE_SIZE / 2) + (WALL_WIDTH / 2);
                            int y = (i * WALL_HEIGHT) - (TILE_SIZE / 2) - (WALL_HEIGHT / 2);
                            Console.WriteLine($"{x}, {y}");
                            Shared.Entities.Entity wall = Shared.Entities.Wall.Create("Images/player", Color.Green, new Rectangle(x, y, WALL_WIDTH, WALL_HEIGHT));
                            worldEntities.Add(wall);
                            Server.MessageQueueServer.instance.broadcastMessage(new Shared.Messages.NewEntity(wall));
                        }

                    }
                    if (i == TILE_COUNT - 1)
                    {
                        int wallCount = TILE_SIZE / WALL_WIDTH;
                        for (int w = 0; w < wallCount; w++)
                        {
                            int x = (j * ((w + 1) * WALL_WIDTH)) - (TILE_SIZE / 2);
                            int y = ((i * WALL_HEIGHT) - (TILE_SIZE) - WALL_HEIGHT) + WORLD_END;
                            Shared.Entities.Entity wall = Shared.Entities.Wall.Create("Images/player", Color.Green, new Rectangle(x, y, WALL_WIDTH, WALL_HEIGHT));
                            worldEntities.Add(wall);
                            Server.MessageQueueServer.instance.broadcastMessage(new Shared.Messages.NewEntity(wall));
                        }

                    }
                    // else if (j == 0 || j == TILE_COUNT - 1)
                    // {
                    //     int wallCount = TILE_SIZE / WALL_HEIGHT;
                    //     // For each tile, we need to render this many walls
                    //     for (int w = 0; w < wallCount; w++)
                    //     {
                    //         Shared.Entities.Entity wall = Shared.Entities.Wall.Create("Images/player", Color.Green, new Rectangle(j * (w + 1) * WALL_WIDTH - (TILE_SIZE / 2), i * (w + 1) * WALL_HEIGHT - (TILE_SIZE / 2), WALL_WIDTH, WALL_HEIGHT));
                    //         worldEntities.Add(wall);
                    //         Server.MessageQueueServer.instance.broadcastMessage(new Shared.Messages.NewEntity(wall));
                    //     }
                    // }
                    if ((i != 0 || i != TILE_COUNT - 1) && (j != 0 || j != TILE_COUNT - 1))
                    {
                        Shared.Entities.Entity tile = Shared.Entities.Tile.Create("Images/Tile1", new Rectangle(j * TILE_SIZE, i * TILE_SIZE, TILE_SIZE, TILE_SIZE), Color.White);
                        worldEntities.Add(tile);
                        Server.MessageQueueServer.instance.broadcastMessage(new Shared.Messages.NewEntity(tile));
                    }
                }
            }
        }
    }
}
