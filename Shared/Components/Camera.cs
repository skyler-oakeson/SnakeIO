using Microsoft.Xna.Framework;
using System;

namespace Shared.Components
{
    public class Camera : Component
    {
        public Rectangle rectangle { get; set; }
        public Point center
        {
            get
            {
                return new Point(rectangle.X + (rectangle.Width / 2), rectangle.Y + (rectangle.Height / 2));
            }
        }

        public Camera(Rectangle rectangle)
        {
            // Rectangle width will be camera / screen width, height is camera / screen height, X and Y are top left
            this.rectangle = rectangle;
        }

        public bool ShouldRender(Shared.Entities.Entity entity)
        {
            return true;
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
