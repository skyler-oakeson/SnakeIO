using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shared.Entities
{
    public class Wall
    {
        public static Entity Create(string texture, Color color, Rectangle rectangle, string chain = null, Components.LinkPosition? linkPos = null)
        {
            Entity wall = new Entity();
            wall.Add(new Components.Appearance(texture, typeof(Texture2D), color, Color.Transparent, rectangle));
            wall.Add(new Components.Positionable(new Vector2(rectangle.X, rectangle.Y), 0f));
            int radius = rectangle.Width >= rectangle.Height ? rectangle.Width / 2 : rectangle.Height / 2;
            Shared.Components.CircleData circleData = new Shared.Components.CircleData { };
            Shared.Components.RectangleData rectangleData = new Shared.Components.RectangleData { x = rectangle.X, y = rectangle.Y, width = rectangle.Width, height = rectangle.Height };
            wall.Add(new Components.Collidable(Shared.Components.CollidableShape.Rectangle, rectangleData, circleData));

            return wall;
        }
    }
}
