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

        // This will be handled in CreateEntity. These are just used as flags
        public override void Serialize(ref List<byte> data)
        {
        }
    }

    public enum CollidableShape
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
