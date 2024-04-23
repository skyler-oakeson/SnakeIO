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
            UInt16 shape = BitConverter.ToUInt16(data, offset);
            offset += sizeof(UInt16);
            float rectX = BitConverter.ToSingle(data, offset);
            offset += sizeof(Single);
            float rectY = BitConverter.ToSingle(data, offset);
            offset += sizeof(Single);
            int rectWidth = BitConverter.ToInt32(data, offset);
            offset += sizeof(Int32);
            int rectHeight = BitConverter.ToInt32(data, offset);
            offset += sizeof(Int32);
            float circleX = BitConverter.ToSingle(data, offset);
            offset += sizeof(Single);
            float circleY = BitConverter.ToSingle(data, offset);
            offset += sizeof(Single);
            int circleRadius = BitConverter.ToInt32(data, offset);
            offset += sizeof(Int32);
            this.message = new CollidableMessage()
            {
                Shape = (Shared.Components.CollidableShape)shape,
                RectangleData = new Shared.Components.RectangleData { x = rectX, y = rectY, width = rectWidth, height = rectHeight },
                CircleData = new Shared.Components.CircleData { x = circleX, y = circleY, radius = circleRadius }
            };
        }

        public CollidableMessage GetMessage()
        {
            return Message;
        }

        public struct CollidableMessage
        {
            public Shared.Components.CollidableShape Shape { get; set; }
            public Shared.Components.RectangleData RectangleData { get; set; }
            public Shared.Components.CircleData CircleData { get; set; }
        }
    }
}
