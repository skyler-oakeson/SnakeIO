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
            int chainNameLength = BitConverter.ToInt32(data, offset);
            offset += sizeof(int);
            string chain = Encoding.ASCII.GetString(data, offset, chainNameLength);
            offset += chainNameLength;
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
