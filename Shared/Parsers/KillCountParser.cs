using Microsoft.Xna.Framework.Audio;
namespace Shared.Parsers
{
    public class KillCountParser : Parser
    {
        private KillCountMessage message { get; set; }
        public KillCountMessage Message
        {
            get { return message; }
            set { message = value; }
        }

        public override void Parse(ref byte[] data, ref int offset)
        {
            int count = BitConverter.ToInt32(data, offset);
            offset += sizeof(Int32);
            this.message = new KillCountMessage {
                count = count
            };
        }

        public KillCountMessage GetMessage()
        {
            return Message;
        }

        public struct KillCountMessage
        {
            public int count { get; set; }
        }
    }
}
