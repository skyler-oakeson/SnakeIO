using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shared.Entities
{
    public class Tile
    {
        public static Entity Create(string texture, Rectangle rectangle, Color color)
        {
            Entity tile = new Entity();
            tile.Add(new Components.Appearance(texture, typeof(Texture2D),  color, Color.Transparent, rectangle));
            tile.Add(new Components.Positionable(new Vector2(rectangle.X, rectangle.Y), 0f));

            return tile;
        }
    }
}
