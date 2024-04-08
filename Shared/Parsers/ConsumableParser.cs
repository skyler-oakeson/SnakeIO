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
            int typeSize = BitConverter.ToInt32(data, offset);
            offset += sizeof(Int32);
            System.Type messageType = System.Type.GetType(Encoding.UTF8.GetString(data, offset, typeSize));
            offset += typeSize;
            this.message = new ConsumableMessage()
            {
                growth = messageGrowth,
                type = messageType
            };
        }

        public ConsumableMessage GetMessage()
        {
            return Message;
        }

        public struct ConsumableMessage 
        {
            public float growth { get; set; }
            public Type type { get; set; }
        }
    }
}
