namespace Shared.Parsers
{
    public class MouseControllableParser : Parser
    {
        private MouseControllableMessage message { get; set; }
        public MouseControllableMessage Message
        {
            get { return message; }
            set { message = value; }
        }
        //TODO: Implement with new input
        public override void Parse(ref byte[] data, ref int offset)
        {
            // Parse the data
        }

        public MouseControllableMessage GetMessage()
        {
            return Message;
        }

        public struct MouseControllableMessage
        {
        }
    }
}
