using Microsoft.Xna.Framework;
using System;

namespace Shared.Components
{
    public class Camera : Component
    {
        public Rectangle rectangle { get; set; }
        public Point center { get; set; }
        public Matrix Transform { get; set; } = Matrix.Identity;
        //Transition speeds
        public float LerpAmount { get; set; } = 0f;
        public float LerpSpeed = .00005f;

        public Camera(Rectangle rectangle)
        {
            // Rectangle width will be camera / screen width, height is camera / screen height, X and Y are top left
            this.rectangle = rectangle;
            this.center = new Point(rectangle.X + (rectangle.Width / 2), rectangle.Y + (rectangle.Height / 2));
            Matrix position = Matrix.CreateTranslation(0, 0, 0);
            Matrix offset = Matrix.CreateTranslation(rectangle.Width / 2, rectangle.Height / 2, 0);
            Transform = position * offset;
        }

        public void Follow(Shared.Entities.Entity target)
        {
            Shared.Components.Renderable renderable = target.GetComponent<Shared.Components.Renderable>();
            Shared.Components.Positionable positionable = target.GetComponent<Shared.Components.Positionable>();
            Matrix position = Matrix.CreateTranslation(-positionable.pos.X - (renderable.rectangle.Width / 2), -positionable.pos.Y - (renderable.rectangle.Height / 2), 0);
            Matrix offset = Matrix.CreateTranslation(rectangle.Width / 2, rectangle.Height / 2, 0);
            Transform = position * offset;
        }

        public bool ShouldRender(Shared.Entities.Entity entity)
        {
            // https://stackoverflow.com/questions/401847/circle-rectangle-collision-detection-intersection
            if (this.LerpAmount >= 1f)
            {
                if (entity.ContainsComponent<Shared.Components.Collidable>())
                {
                    if (entity.GetComponent<Shared.Components.Collidable>().Data.Shape == Shared.Components.CollidableShape.Rectangle)
                    {
                        return RectangleIntersection(entity);
                    }
                    else
                    {
                        return CircleRectangleIntersection(entity);
                    }
                }
                else
                {
                    return CircleRectangleIntersection(entity);
                }
            }
            return true;
        }

        private bool RectangleIntersection(Shared.Entities.Entity entity)
        {
            Shared.Components.Collidable collidable = entity.GetComponent<Shared.Components.Collidable>();

            double entityRectTop = collidable.Data.RectangleData.y - collidable.Data.RectangleData.width / 2;
            double entityRectLeft = collidable.Data.RectangleData.x - collidable.Data.RectangleData.height / 2;
            double entityRectRight = collidable.Data.RectangleData.x + collidable.Data.RectangleData.height / 2;;
            double entityRectBottom = collidable.Data.RectangleData.y + collidable.Data.RectangleData.width / 2;;
            double cameraTop = rectangle.Y - rectangle.Height / 2;
            double cameraLeft = rectangle.X - rectangle.Width / 2;;
            double cameraRight = rectangle.X + rectangle.Width / 2;;
            double cameraBottom = rectangle.Y + rectangle.Height / 2;;

            bool xIntersect = entityRectLeft < cameraRight && entityRectRight > cameraLeft;
            bool yIntersect = entityRectTop < cameraBottom && entityRectBottom > cameraTop;

            return xIntersect && yIntersect;
        }

        private bool CircleRectangleIntersection(Shared.Entities.Entity entity)
        {
            Shared.Components.Renderable renderable = entity.GetComponent<Shared.Components.Renderable>();
            Shared.Components.Positionable positionable = entity.GetComponent<Shared.Components.Positionable>();
            int radius = renderable.texture.Width >= renderable.texture.Height ? renderable.texture.Width / 2 : renderable.texture.Height / 2;
            double circleDistanceX = Math.Abs(positionable.pos.X - rectangle.X);
            double circleDistanceY = Math.Abs(positionable.pos.Y - rectangle.Y);
            if (circleDistanceX > ((rectangle.Width + 50) / 2 + radius))
            {
                return false;
            }
            if (circleDistanceY > ((rectangle.Height + 50) / 2 + radius))
            {
                return false;
            }
            if (circleDistanceX <= ((rectangle.Width + 50) / 2))
            {
                return true;
            }
            if (circleDistanceY <= ((rectangle.Height + 50) / 2))
            {
                return true;
            }
            double cornerDistanceSQ = (circleDistanceX - (rectangle.Width + 50) / 2) * (circleDistanceX - (rectangle.Width + 50) / 2) + (circleDistanceY - (rectangle.Height + 50) / 2) * (circleDistanceY - (rectangle.Height + 50) / 2);
            return (cornerDistanceSQ <= (radius * radius));

        }

        //TODO: Probably remove this, we make all the data in the createEntity anyways
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
