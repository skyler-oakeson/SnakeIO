using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shared.Entities
{
    public class Tile
    {
        public static Entity Create(string texturePath, Rectangle rectangle, Color color)
        {
            Entity tile = new Entity();
            tile.Add(new Components.Appearance());
            tile.Add(new Components.Renderable(color, Color.Transparent, rectangle, texturePath: texturePath));
            tile.Add(new Components.Positionable(new Vector2(rectangle.X, rectangle.Y), 0f));

            return tile;
        }
    }
}
