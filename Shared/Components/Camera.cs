using Microsoft.Xna.Framework;
using System;

namespace Shared.Components
{
    public class Camera : Component
    {
        public Rectangle rectangle { get; set; }
        public Point center { get; set; }

        public Camera(Rectangle rectangle)
        {
            // Rectangle width will be camera / screen width, height is camera / screen height, X and Y are top left
            this.rectangle = rectangle;
            this.center = new Point(rectangle.X + (rectangle.Width / 2), rectangle.Y + (rectangle.Height / 2));
        }

        public bool ShouldRender(Shared.Entities.Entity entity)
        {
            // https://stackoverflow.com/questions/401847/circle-rectangle-collision-detection-intersection
            if (entity.ContainsComponent<Shared.Components.Collidable>())
            {
                Shared.Components.Collidable collidable = entity.GetComponent<Shared.Components.Collidable>();
                double circleDistanceX = Math.Abs(collidable.hitBox.X - rectangle.X);
                double circleDistanceY = Math.Abs(collidable.hitBox.Y - rectangle.Y);
                if (circleDistanceX > (rectangle.Width / 2 + collidable.hitBox.Z))
                {
                    return false;
                }
                if (circleDistanceY > (rectangle.Height / 2 + collidable.hitBox.Z))
                {
                    return false;
                }
                if (circleDistanceX > (rectangle.Width / 2))
                {
                    return true;
                }
                if (circleDistanceX > (rectangle.Width / 2))
                {
                    return true;
                }
                double cornerDistanceSQ = (circleDistanceX - rectangle.Width / 2) * (circleDistanceX - rectangle.Width / 2) + (circleDistanceY - rectangle.Height / 2) * (circleDistanceY - rectangle.Height / 2);

                return (cornerDistanceSQ <= (collidable.hitBox.Z * collidable.hitBox.Z));
            }
            return false;
        }

        // This will be handled in CreateEntity. These are just used as flags
        public override void Serialize(ref List<byte> data)
        {
            data.AddRange(BitConverter.GetBytes(rectangle.X));
            data.AddRange(BitConverter.GetBytes(rectangle.Y));
            data.AddRange(BitConverter.GetBytes(rectangle.Width));
            data.AddRange(BitConverter.GetBytes(rectangle.Height));
            data.AddRange(BitConverter.GetBytes(center.X));
            data.AddRange(BitConverter.GetBytes(center.Y));
        }
    }
}
