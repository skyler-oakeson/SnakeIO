using System;

namespace Components
{
    class Spawnable : Component
    {
        public TimeSpan spawnRate; // in milliseconds
        public int spawnCount; // number of entities to spawn

        public Spawnable(TimeSpan spawnRate, int spawnCount)
        {
            this.spawnRate = spawnRate;
            this.spawnCount = spawnCount;
        }
    }
}
