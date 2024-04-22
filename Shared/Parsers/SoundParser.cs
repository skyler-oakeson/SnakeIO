using System.Text;
namespace Shared.Parsers
{
    public class SoundParser : Parser
    {

        private SoundMessage message { get; set; }
        public SoundMessage Message
        {
            get { return message; }
            set { message = value; }
        }

        public override void Parse(ref byte[] data, ref int offset)
        {
            int soundPathLength = BitConverter.ToInt32(data, offset);
            offset += sizeof(Int32);
            string soundPath = Encoding.ASCII.GetString(data, offset, soundPathLength);
            offset += soundPathLength;
            this.message = new SoundMessage
            {
                soundPath = soundPath
            };
        }

        public SoundMessage GetMessage()
        {
            return Message;
        }

        public struct SoundMessage
        {
            public string soundPath;
        }
    }
}
