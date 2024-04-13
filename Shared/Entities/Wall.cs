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
            // int radius = texture.Width >= texture.Height ? texture.Height/2 : texture.Width/2;

            // wall.Add(new Components.Renderable(texture, "Images/player", color, Color.Black, rectangle));
            wall.Add(new Components.Appearance(texture, typeof(Texture2D), color, Color.Transparent, rectangle));
            wall.Add(new Components.Positionable(new Vector2(rectangle.X, rectangle.Y)));

            return wall;
        }
    }
}
