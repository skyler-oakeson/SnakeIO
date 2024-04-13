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
            UInt16 type = BitConverter.ToUInt16(data, offset);
            offset += sizeof(UInt16);
            this.message = new KeyboardControllableMessage() 
            {
                enable = enable,
                type = (Shared.Controls.ControlableEntity)type
            };
        }

        public KeyboardControllableMessage GetMessage()
        {
            return Message;
        }

        public struct KeyboardControllableMessage
        {
            public bool enable { get; set; }
            public Shared.Controls.ControlableEntity type { get; set; }
        }
    }
}
