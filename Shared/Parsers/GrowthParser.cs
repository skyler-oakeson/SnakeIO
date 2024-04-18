using System.Text;
namespace Shared.Parsers
{
    public class GrowthParser : Parser
    {
        private GrowthMessage message { get; set; }
        public GrowthMessage Message
        {
            get { return message; }
            set { message = value; }
        }

        public override void Parse(ref byte[] data, ref int offset)
        {
            float messageGrowth = BitConverter.ToSingle(data, offset);
            offset += sizeof(Single);
            this.message = new GrowthMessage()
            {
                growth = messageGrowth,
            };
        }

        public GrowthMessage GetMessage()
        {
            return Message;
        }

        public struct GrowthMessage
        {
            public float growth { get; set; }
        }
    }
}
