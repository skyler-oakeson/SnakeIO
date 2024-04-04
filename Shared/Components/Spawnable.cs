using System;

namespace Shared.Components
{
    class Spawnable : Component
    {
        public TimeSpan spawnRate; // in milliseconds
        public int spawnCount; // number of entities to spawn
        public Type type;

        public Spawnable(TimeSpan spawnRate, int spawnCount, Type type)
        {
            this.spawnRate = spawnRate;
            this.spawnCount = spawnCount;
            this.type = type;
        }
    }
}
