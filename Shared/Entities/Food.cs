using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using Shared.Entities;

namespace Shared.Entities
{
    public class Food
    {
        private static MyRandom random = new MyRandom();
        public static Entity Create(string texture, Rectangle rectangle)
        {
            Entity food = new Entity();

            int r = random.Next(0, 255);
            int g = random.Next(0, 255);
            int b = random.Next(0, 255);

            food.Add(new Components.Appearance("Images/food", typeof(Texture2D), new Color(r, g, b), Color.White, rectangle));
            food.Add(new Components.Animatable(new int[] { 200, 200, 200, 200, 200, 200 }));
            food.Add(new Shared.Components.Positionable(new Vector2(rectangle.X, rectangle.Y), 0f));
            food.Add(new Components.Consumable(1.0f));
            food.Add(new Components.Spawnable(TimeSpan.FromMilliseconds(5000), 25, typeof(Food)));

            int radius = rectangle.Width >= rectangle.Height ? rectangle.Width / 2 : rectangle.Height / 2;
            Shared.Components.CircleData circleData = new Shared.Components.CircleData { x = rectangle.X, y = rectangle.Y, radius = radius };
            Shared.Components.RectangleData rectangleData = new Shared.Components.RectangleData { };
            food.Add(new Components.Collidable(Shared.Components.CollidableShape.Circle, rectangleData, circleData));

            return food;
        }
    }
}
