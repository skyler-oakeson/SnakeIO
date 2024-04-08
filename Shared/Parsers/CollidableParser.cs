using Microsoft.Xna.Framework;
namespace Shared.Parsers
{
    public class CollidableParser : Parser
    {
        private CollidableMessage message { get; set; }
        public CollidableMessage Message
        {
            get { return message; }
            set { message = value; }
        }

        //Intentionally left blank
        public override void Parse(ref byte[] data, ref int offset)
        {
        }

        public CollidableMessage GetMessage()
        {
            return Message;
        }

        public struct CollidableMessage
        {
            public Vector3 hitBox { get; set; }
            public bool collided { get; set; }
        }
    }
}
