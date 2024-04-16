using System.Text;

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
            int typeSize = BitConverter.ToInt32(data, offset);
            Console.WriteLine(typeSize);
            offset += sizeof(Int32);
            System.Type type = System.Type.GetType(Encoding.UTF8.GetString(data, offset, typeSize));
            offset += typeSize;
            Console.WriteLine(type);
            this.message = new KeyboardControllableMessage() 
            {
                enable = enable,
                type = type
            };
        }

        public KeyboardControllableMessage GetMessage()
        {
            return Message;
        }

        public struct KeyboardControllableMessage
        {
            public bool enable { get; set; }
            public Type type { get; set; }
        }
    }
}
