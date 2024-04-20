
using System.Text;
namespace Shared.Parsers
{
    public class SnakeIDParser : Parser
    {
        private SnakeIDMessage message { get; set; }
        public SnakeIDMessage Message
        {
            get { return message; }
            set { message = value; }
        }

        public override void Parse(ref byte[] data, ref int offset)
        {
            int id  = BitConverter.ToInt32(data, offset);
            offset += sizeof(Int32);
            this.message = new SnakeIDMessage()
            {
                id = id,
            };
        }

        public SnakeIDMessage GetMessage()
        {
            return Message;
        }

        public struct SnakeIDMessage
        {
            public int id { get; set; }
        }
    }
}
