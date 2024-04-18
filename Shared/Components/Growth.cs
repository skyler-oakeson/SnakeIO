using System;
using System.Text;

namespace Shared.Components
{
    public class Growth : Component
    {
        public float growth;

        public Growth()
        {
            this.growth = 0;
        }

        public override void Serialize(ref List<byte> data)
        {
            data.AddRange(BitConverter.GetBytes(growth));
        }
    }
}
