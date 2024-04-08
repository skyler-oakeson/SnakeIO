using Microsoft.Xna.Framework;
namespace Shared.Parsers 
{
    public class MovableParser : Parser 
    {
        private MovableMessage message { get; set; }
        public MovableMessage Message
        {
            get { return message; }
            set { message = value; }
        }

        public override void Parse(ref byte[] data, ref int offset) {
            float rotationX = BitConverter.ToSingle(data, offset);
            offset += sizeof(Single);
            float rotationY = BitConverter.ToSingle(data, offset);
            offset += sizeof(Single);
            float velocityX = BitConverter.ToSingle(data, offset);
            offset += sizeof(Single);
            float velocityY = BitConverter.ToSingle(data, offset);
            offset += sizeof(Single);
            this.message = new MovableMessage() {
                rotation = new Vector2(rotationX, rotationY),
                velocity = new Vector2(velocityX, velocityY)
            };
        }

        public MovableMessage GetMessage()
        {
            return Message;
        }

        public struct MovableMessage
        {
            public Vector2 rotation { get; set; }
            public Vector2 velocity { get; set; }
        }
    }
}
