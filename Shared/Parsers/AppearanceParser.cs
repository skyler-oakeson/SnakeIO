using System;
using Microsoft.Xna.Framework;
using System.Text;
namespace Shared.Parsers
{
    public class AppearanceParser : Parser
    {
        private AppearanceMessage message { get; set; }
        public AppearanceMessage Message
        {
            get { return message; }
            set { message = value; }
        }

        public override void Parse(ref byte[] data, ref int offset)
        {
            int textureSize = BitConverter.ToInt32(data, offset);
            offset += sizeof(Int32);
            string messageTexturePath = Encoding.UTF8.GetString(data, offset, textureSize);
            offset += textureSize;
            int typeSize = BitConverter.ToInt32(data, offset);
            offset += sizeof(Int32);
            System.Type messageType = System.Type.GetType(Encoding.UTF8.GetString(data, offset, typeSize));
            offset += typeSize;
            //for color
            int colorR = data[offset];
            offset += sizeof(int);
            int colorG = data[offset];
            offset += sizeof(int);
            int colorB = data[offset];
            offset += sizeof(int);
            int colorA = data[offset];
            offset += sizeof(int);
            int strokeR = data[offset];
            offset += sizeof(int);
            int strokeG = data[offset];
            offset += sizeof(int);
            int strokeB = data[offset];
            offset += sizeof(int);
            int strokeA = data[offset];
            offset += sizeof(int);
            this.message = new AppearanceMessage
            {
                texturePath = messageTexturePath,
                color = new Color(colorR, colorG, colorB, colorA),
                stroke = new Color(strokeR, strokeG, strokeB, strokeA),
                type = messageType
            };
        }

        public AppearanceMessage GetMessage()
        {
            return Message;
        }

        public struct AppearanceMessage
        {
            public string texturePath { get; set; }
            public Color color { get; set; }
            public Color stroke { get; set; }
            public Type type { get; set; }
        }
    }
}
