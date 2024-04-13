using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Utils
{
    public class WorldGenerator
    {

        private List<Shared.Entities.Entity> worldEntities = new List<Shared.Entities.Entity>();
        private const int TILE_SIZE = 50;
        private const int WALL_WIDTH = 50;
        private const int WALL_HEIGHT = 10;
        private const int WORLD_SIZE = 1;
        private const int WORLD_ORIGIN = 0;
        private const int WORLD_END = 1000;
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
                    Shared.Entities.Entity tile = Shared.Entities.Tile.Create("Images/player", new Rectangle(i * TILE_SIZE, j * TILE_SIZE, 50, 50), Color.Yellow);
                    worldEntities.Add(tile);
                    Server.MessageQueueServer.instance.broadcastMessage(new Shared.Messages.NewEntity(tile));
                    if (i == 0 || i == TILE_COUNT - 1)
                    {
                        Shared.Entities.Entity wall = Shared.Entities.Wall.Create("Images/player", Color.Green, new Rectangle(i * TILE_SIZE, j * TILE_SIZE, 50, 50));
                        worldEntities.Add(wall);
                        Server.MessageQueueServer.instance.broadcastMessage(new Shared.Messages.NewEntity(wall));

                    }
                    if (j == 0 || j == TILE_COUNT - 1)
                    {
                        Shared.Entities.Entity wall = Shared.Entities.Wall.Create("Images/player", Color.Green, new Rectangle(i * TILE_SIZE, j * TILE_SIZE, 50, 50));
                        worldEntities.Add(wall);
                        Server.MessageQueueServer.instance.broadcastMessage(new Shared.Messages.NewEntity(wall));
                    }
                }
            }
        }
    }
}
