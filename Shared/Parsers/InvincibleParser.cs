
using Microsoft.Xna.Framework.Audio;
namespace Shared.Parsers
{
    public class InvincibleParser : Parser
    {
        private InvincibleMessage message { get; set; }
        public InvincibleMessage Message
        {
            get { return message; }
            set { message = value; }
        }

        public override void Parse(ref byte[] data, ref int offset)
        {
            int rate = BitConverter.ToInt32(data, offset);
            TimeSpan messageTime = new TimeSpan(rate);
            offset += sizeof(Int32);
            this.message = new InvincibleMessage {
                time = messageTime
            };
        }

        public InvincibleMessage GetMessage()
        {
            return Message;
        }

        public struct InvincibleMessage
        {
            public TimeSpan time { get; set; }
        }
    }
}
