using System.Text;
namespace Shared.Parsers
{
    public class GrowableParser : Parser
    {
        private GrowableMessage message { get; set; }
        public GrowableMessage Message
        {
            get { return message; }
            set { message = value; }
        }

        public override void Parse(ref byte[] data, ref int offset)
        {
            float messageGrowable = BitConverter.ToSingle(data, offset);
            offset += sizeof(Single);
            this.message = new GrowableMessage()
            {
                growth = messageGrowable,
            };
        }

        public GrowableMessage GetMessage()
        {
            return Message;
        }

        public struct GrowableMessage
        {
            public float growth { get; set; }
        }
    }
}
