using System;
using System.Text;

namespace Shared.Components
{
    public class Spawnable : Component
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

        public override void Serialize(ref List<byte> data)
        {
            data.AddRange(BitConverter.GetBytes((int) spawnRate.TotalMilliseconds));
            data.AddRange(BitConverter.GetBytes(spawnCount));
            data.AddRange(BitConverter.GetBytes(type.ToString().Length));
            data.AddRange(Encoding.UTF8.GetBytes(type.ToString()));
        }
    }
}
