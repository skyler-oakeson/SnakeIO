using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Entities
{
    public class Food
    {
        public static Entity Create(Texture2D texture, Vector2 pos)
        {
            Entity food = new Entity();

            int radius = texture.Width >= texture.Height ? texture.Width/2 : texture.Height/2;

            food.Add(new Components.Renderable(texture, Color.Blue, Color.Black));
            food.Add(new Components.Positionable(pos));
            food.Add(new Components.Consumable(typeof(Food), 1.0f));
            food.Add(new Components.Spawnable(TimeSpan.FromMilliseconds(5000), 10, typeof(Food)));
            food.Add(new Components.Collidable(new Vector3(pos.X, pos.Y, radius-30)));

            return food;
        }
    }
}
