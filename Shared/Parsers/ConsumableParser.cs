using System.Text;
namespace Shared.Parsers
{
    public class ConsumableParser : Parser
    {
        private ConsumableMessage message { get; set; }
        public ConsumableMessage Message
        {
            get { return message; }
            set { message = value; }
        }

        public override void Parse(ref byte[] data, ref int offset)
        {
            float messageGrowth = BitConverter.ToSingle(data, offset);
            offset += sizeof(Single);
            this.message = new ConsumableMessage()
            {
                growth = messageGrowth,
            };
        }

        public ConsumableMessage GetMessage()
        {
            return Message;
        }

        public struct ConsumableMessage
        {
            public float growth { get; set; }
        }
    }
}
