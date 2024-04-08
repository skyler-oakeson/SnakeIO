using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Shared.Parsers
{
    public class RenderableParser : Parser 
    {
        private RenderableMessage message { get; set; }
        public RenderableMessage Message
        {
            get { return message; }
            set { message = value; }
        }
        
        // Intentionally left blank
        public override void Parse(ref byte[] data, ref int offset)
        {
        }

        public RenderableMessage GetMessage()
        {
            return Message;
        }

        public struct RenderableMessage
        {
            public Texture2D texture { get; set; }
            public Color color { get; set; }
            public Color stroke { get; set; }
        }
    }
}
