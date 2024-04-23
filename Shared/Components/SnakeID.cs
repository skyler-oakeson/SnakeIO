
using System.Text;

namespace Shared.Components
{
    public class SnakeID : Component
    {
        public int id { get; private set; }
        public string name { get; private set; }

        public SnakeID(int id, string name)
        {
            this.id = id;
            this.name = name;
        }


        public override void Serialize(ref List<byte> data)
        {
            data.AddRange(BitConverter.GetBytes(id));
            data.AddRange(BitConverter.GetBytes(name.Length));
            data.AddRange(Encoding.UTF8.GetBytes(name));
        }
    }
}

