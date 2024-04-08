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
        }

        public KeyboardControllableMessage GetMessage()
        {
            return Message;
        }

        public struct KeyboardControllableMessage
        {
        }
    }
}
