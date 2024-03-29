
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Entities
{
    public class Food
    {
        public static Entity Create(Texture2D texture, Vector2 pos)
        {
            Entity food = new Entity();

            food.Add(new Components.Renderable(texture, Color.White, Color.Black));
            food.Add(new Components.Positionable(pos));
            food.Add(new Components.Consumable(1.0f));
            food.Add(new Components.Spawnable(1.0f));
            //add collidable component

            return food;
        }
    }
}
