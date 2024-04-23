using System;
using Microsoft.Xna.Framework;
namespace Shared.Parsers
{
    public class CameraParser : Parser
    {

        private CameraMessage message { get; set; }
        public CameraMessage Message
        {
            get { return message; }
            set { message = value; }
        }

        public override void Parse(ref byte[] data, ref int offset)
        {
            int rectangleX = BitConverter.ToInt32(data, offset);
            offset += sizeof(Int32);
            int rectangleY = BitConverter.ToInt32(data, offset);
            offset += sizeof(Int32);
            int rectangleWidth = BitConverter.ToInt32(data, offset);
            offset += sizeof(Int32);
            int rectangleHeight = BitConverter.ToInt32(data, offset);
            offset += sizeof(Int32);
            int centerX = BitConverter.ToInt32(data, offset);
            offset += sizeof(Int32);
            int centerY = BitConverter.ToInt32(data, offset);
            offset += sizeof(Int32);
            this.message = new CameraMessage()
            {
                rectangle = new Rectangle(rectangleX, rectangleY, rectangleWidth, rectangleHeight),
                center = new Point(centerX, centerY)
            };

        }

        public CameraMessage GetMessage()
        {
            return Message;
        }

        public struct CameraMessage
        {
            public Rectangle rectangle;
            public Point center;
        }
    }
}
