namespace Shared.Parsers
{
    public class KeyboardControllableParser : Parser
    {
        private KeyboardControllableMessage message { get; set; }
        public KeyboardControllableMessage Message
        {
            get { return message; }
            set { message = value; }
        }
        //TODO: Implement with new input
        public override void Parse(ref byte[] data, ref int offset)
        {
            // Parse the data
            bool enable = BitConverter.ToBoolean(data, offset);
            offset += sizeof(Boolean);
            this.message = new KeyboardControllableMessage() 
            {
                enable = enable,
            };
        }

        public KeyboardControllableMessage GetMessage()
        {
            return Message;
        }

        public struct KeyboardControllableMessage
        {
            public bool enable { get; set; }
        }
    }
}
