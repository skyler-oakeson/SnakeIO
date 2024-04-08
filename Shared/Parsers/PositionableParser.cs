using Microsoft.Xna.Framework;
namespace Shared.Parsers
{

    public class PositionableParser : Parser
    {
        private PositionableMessage message { get; set; }
        public PositionableMessage Message
        {
            get { return message; }
            set { message = value; }
        }

        public override void Parse(ref byte[] data, ref int offset)
        {
            float posX = BitConverter.ToSingle(data, offset);
            offset += sizeof(Single);
            float posY = BitConverter.ToSingle(data, offset);
            offset += sizeof(Single);
            this.message = new PositionableMessage()
            {
                pos = new Vector2(posX, posY)
            };
        }

        public PositionableMessage GetMessage()
        {
            return Message;
        }

        public struct PositionableMessage
        {
            public Vector2 pos { get; set; }
        }
    }
}
