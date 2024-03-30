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

            food.Add(new Components.Renderable(texture, Color.Blue, Color.Black));
            Debug.WriteLine("Food Created");
            food.Add(new Components.Positionable(pos));
            food.Add(new Components.Consumable(typeof(Food), 1.0f));
            food.Add(new Components.Spawnable(TimeSpan.FromMilliseconds(5000), 10, typeof(Food)));
            food.Add(new Components.Collidable(new Vector3(0, 0, 0)));

            return food;
        }
    }
}
