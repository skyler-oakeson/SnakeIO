using System;
using System.Text;

namespace Shared.Components
{
    public class Consumable : Component
    {
        public float growth;
        public Type type { get; private set; }

        public Consumable(Type type, float growth)
        {
            this.growth = growth;
            this.type = type;
        }

        public override void Serialize(ref List<byte> data)
        {
            data.AddRange(BitConverter.GetBytes(growth));
            data.AddRange(BitConverter.GetBytes(type.AssemblyQualifiedName.Length));
            data.AddRange(Encoding.UTF8.GetBytes(type.AssemblyQualifiedName));
        }
    }
}
