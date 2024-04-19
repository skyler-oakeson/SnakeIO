using System;
using System.Text;

namespace Shared.Components
{
    public class Growable : Component
    {
        public float growth { get; set; }

        public Growable()
        {
            this.growth = 0;
        }

        public override void Serialize(ref List<byte> data)
        {
            data.AddRange(BitConverter.GetBytes(growth));
        }
    }
}
