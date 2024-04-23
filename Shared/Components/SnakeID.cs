
using System.Text;

namespace Shared.Components
{
    public class SnakeID : Component
    {
        public int id { get; private set; }

        public SnakeID(int id)
        {
            this.id = id;
        }


        public override void Serialize(ref List<byte> data)
        {
            data.AddRange(BitConverter.GetBytes(id));
        }
    }
}

