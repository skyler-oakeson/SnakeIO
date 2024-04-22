using System;
using Microsoft.Xna.Framework;
using System.Text;
namespace Shared.Parsers
{
    public class ReadableParser : Parser
    {
        private ReadableMessage message { get; set; }
        public ReadableMessage Message
        {
            get { return message; }
            set { message = value; }
        }

        public override void Parse(ref byte[] data, ref int offset)
        {
            int fontSize = BitConverter.ToInt32(data, offset);
            offset += sizeof(Int32);
            string messageFontPath = Encoding.UTF8.GetString(data, offset, fontSize);
            offset += fontSize;
            int textSize = BitConverter.ToInt32(data, offset);
            offset += sizeof(Int32);
            string messageText = Encoding.UTF8.GetString(data, offset, textSize);
            offset += textSize;
            int typeSize = BitConverter.ToInt32(data, offset);
            offset += sizeof(Int32);
            //rectangle
            int rectangleX = BitConverter.ToInt32(data, offset);
            offset += sizeof(Int32);
            int rectangleY = BitConverter.ToInt32(data, offset);
            offset += sizeof(Int32);
            int rectangleHeight = BitConverter.ToInt32(data, offset);
            offset += sizeof(Int32);
            int rectangleWidth = BitConverter.ToInt32(data, offset);
            offset += sizeof(Int32);
            Rectangle messageRectangle = new Rectangle(rectangleX, rectangleY, rectangleWidth, rectangleHeight);
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
            this.message = new ReadableMessage
            {
                fontPath = messageFontPath,
                text = messageText,
                color = new Color(colorR, colorG, colorB, colorA),
                stroke = new Color(strokeR, strokeG, strokeB, strokeA),
                rectangle = messageRectangle,
            };
        }

        public ReadableMessage GetMessage()
        {
            return Message;
        }

        public struct ReadableMessage
        {
            public string fontPath { get; set; }
            public string text { get; set; }
            public Type type { get; set; }
            public Rectangle rectangle { get; set; }
            public Color color { get; set; }
            public Color stroke { get; set; }
        }
    }
}
