using Microsoft.Xna.Framework;

namespace Shared.Components
{
    /// <summary>
    /// This component is responsible for managing the state of Collidable entities.
    /// </summary>
    public class Collidable : Component
    {
        public CollidableData Data { get; set; }

        public Collidable(CollidableShape shape, RectangleData rectangle, CircleData circle)
        {
            this.Data = new CollidableData
            {
                Shape = shape,
                RectangleData = rectangle,
                CircleData = circle
            };
        }

        public override void Serialize(ref List<byte> data)
        {
            data.AddRange(BitConverter.GetBytes((UInt16) Data.Shape));
            data.AddRange(BitConverter.GetBytes(Data.RectangleData.x));
            data.AddRange(BitConverter.GetBytes(Data.RectangleData.y));
            data.AddRange(BitConverter.GetBytes(Data.RectangleData.width));
            data.AddRange(BitConverter.GetBytes(Data.RectangleData.height));
            data.AddRange(BitConverter.GetBytes(Data.CircleData.x));
            data.AddRange(BitConverter.GetBytes(Data.CircleData.y));
            data.AddRange(BitConverter.GetBytes(Data.CircleData.radius));
        }
    }

    public enum CollidableShape : UInt16
    {
        Rectangle,
        Circle
    }

    public struct RectangleData
    {
        public float x;
        public float y;
        public int width;
        public int height;
    }

    public struct CircleData
    {
        public float x;
        public float y;
        public int radius;
    }

    public class CollidableData
    {
        public CollidableShape Shape;
        public RectangleData RectangleData;
        public CircleData CircleData;
    }
}
