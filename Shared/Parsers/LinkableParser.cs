using System.Text;
namespace Shared.Parsers
{
    public class LinkableParser : Parser
    {
        private LinkableMessage message { get; set; }
        public LinkableMessage Message
        {
            get { return message; }
            set { message = value; }
        }

        public override void Parse(ref byte[] data, ref int offset)
        {
            int chainIdLength = BitConverter.ToInt32(data, offset);
            offset += sizeof(Int32);
            string chain = Encoding.UTF8.GetString(data, offset, 6);
            offset += chainIdLength;
            UInt16 linkPos = BitConverter.ToUInt16(data, offset);
            offset += sizeof(UInt16);

            this.message = new LinkableMessage() 
            {
                chain = chain,
                linkPos = (Shared.Components.LinkPosition)linkPos,
            };
        }

        public LinkableMessage GetMessage()
        {
            return Message;
        }

        public struct LinkableMessage 
        {
            public string chain { get; set; }
            public Shared.Components.LinkPosition linkPos { get; set; }
        }
    }
}
