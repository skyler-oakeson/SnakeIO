using System;
using System.Text;

namespace Shared.Components
{
    public class Consumable : Component
    {
        public float growth;

        public Consumable(float growth)
        {
            this.growth = growth;
        }

        public override void Serialize(ref List<byte> data)
        {
            data.AddRange(BitConverter.GetBytes(growth));
        }
    }
}
