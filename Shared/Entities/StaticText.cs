using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Shared.Entities
{
    public class StaticText 
    {
        public static Entity Create(
                SpriteFont font,
                string value,
                Color color,
                Color stroke,
                Rectangle rectangle)
        {
            Entity staticText = new Entity();
            staticText.Add(new Shared.Components.Appearance());
            staticText.Add(new Shared.Components.Readable(value.ToString(), color, stroke, rectangle, font: font));
            staticText.Add(new Shared.Components.Positionable(new Vector2(rectangle.X, rectangle.Y), 0f));

            return staticText;
        }
    }
}
