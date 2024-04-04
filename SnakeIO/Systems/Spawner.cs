using System;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Systems
{
    class Spawner : System
    {
        private Dictionary<Type, TimeSpan> spawnTimes = new Dictionary<Type, TimeSpan>();
        private List<Entities.Entity> entitiesToSpawn = new List<Entities.Entity>();
        private MyRandom random = new MyRandom();
        private SnakeIO.GameModel.AddDelegate addEntity;

        public Spawner(SnakeIO.GameModel.AddDelegate addEntity)
            : base(
                    typeof(Components.Spawnable),
                    typeof(Components.Positionable))
        {
            this.addEntity = addEntity;
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var entity in entities.Values)
            {
                Components.Spawnable spawnable = entity.GetComponent<Components.Spawnable>();
                if (!spawnTimes.ContainsKey(spawnable.type))
                {
                    spawnTimes[spawnable.type] = gameTime.TotalGameTime - spawnable.spawnRate; //spawn initial count
                }
                if (gameTime.TotalGameTime - spawnTimes[spawnable.type] >= spawnable.spawnRate)
                {
                    spawnTimes[spawnable.type] = gameTime.TotalGameTime + spawnable.spawnRate;
                    SpawnEntity(entity);
                }
            }
            foreach (var entity in entitiesToSpawn)
            {
                addEntity(entity);
            }
            entitiesToSpawn.Clear();
        }

        private void SpawnEntity(Entities.Entity entity)
        {
            Components.Spawnable spawnable = entity.GetComponent<Components.Spawnable>();
            Components.Renderable renderable = entity.GetComponent<Components.Renderable>();
            Type spawnableType = spawnable.type;
            MethodInfo createMethod = spawnableType.GetMethod("Create");
            for (int i = 0; i < spawnable.spawnCount; i++)
            { 
                // There is probably a better way to do this by designing an interface that has the Create() method, then forcing the type to be of that interface.
                // https://learn.microsoft.com/en-us/dotnet/api/system.reflection.methodinfo.invoke?view=netframework-1.1
                // Ensure Create Method exists, and then invoke it here.
                entitiesToSpawn.Add((Entities.Entity)createMethod.Invoke(null, new object[] { renderable.texture, new Vector2((float) random.nextGaussian(100, 50), (float) random.nextGaussian(100, 50)) }));
            }
        }
    }
}
