using Microsoft.Xna.Framework.Audio;
namespace Shared.Parsers
{
    public class AudibleParser : Parser
    {
        private AudibleMessage message { get; set; }
        public AudibleMessage Message
        {
            get { return message; }
            set { message = value; }
        }
        // Intentionally left blank
        public override void Parse(ref byte[] data, ref int offset)
        {
        }

        public AudibleMessage GetMessage()
        {
            return Message;
        }

        public struct AudibleMessage
        {
            public SoundEffect sound { get; set; }
            public bool play { get; set; }
        }
    }
}
