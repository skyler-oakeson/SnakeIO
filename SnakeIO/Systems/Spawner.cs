using System;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Systems
{
    class Spawner : Shared.Systems.System
    {
        private Dictionary<Type, TimeSpan> spawnTimes = new Dictionary<Type, TimeSpan>();
        private List<Shared.Entities.Entity> entitiesToSpawn = new List<Shared.Entities.Entity>();
        private Shared.MyRandom random = new Shared.MyRandom();
        private SnakeIO.GameModel.AddDelegate addEntity;

        public Spawner(SnakeIO.GameModel.AddDelegate addEntity)
            : base( typeof(Shared.Components.Spawnable),
                    typeof(Shared.Components.Positionable))
        {
            this.addEntity = addEntity;
        }

        public override void Update(TimeSpan elapsedTime)
        {
            foreach (var entity in entities.Values)
            {
                Shared.Components.Spawnable spawnable = entity.GetComponent<Shared.Components.Spawnable>();
                if (!spawnTimes.ContainsKey(spawnable.type))
                {
                    spawnTimes[spawnable.type] = elapsedTime - spawnable.spawnRate; //spawn initial count
                }
                if (elapsedTime - spawnTimes[spawnable.type] >= spawnable.spawnRate)
                {
                    spawnTimes[spawnable.type] = elapsedTime + spawnable.spawnRate;
                    SpawnEntity(entity);
                }
            }
            foreach (var entity in entitiesToSpawn)
            {
                addEntity(entity);
            }
            entitiesToSpawn.Clear();
        }

        private void SpawnEntity(Shared.Entities.Entity entity)
        {
            Shared.Components.Spawnable spawnable = entity.GetComponent<Shared.Components.Spawnable>();
            Shared.Components.Renderable renderable = entity.GetComponent<Shared.Components.Renderable>();
            Type spawnableType = spawnable.type;
            MethodInfo createMethod = spawnableType.GetMethod("Create");
            for (int i = 0; i < spawnable.spawnCount; i++)
            { 
                // There is probably a better way to do this by designing an interface that has the Create() method, then forcing the type to be of that interface.
                // https://learn.microsoft.com/en-us/dotnet/api/system.reflection.methodinfo.invoke?view=netframework-1.1
                // Ensure Create Method exists, and then invoke it here.
                entitiesToSpawn.Add((Shared.Entities.Entity)createMethod.Invoke(null, new object[] { renderable.texture, new Vector2((float) random.nextGaussian(100, 50), (float) random.nextGaussian(100, 50)) }));
            }
        }
    }
}
