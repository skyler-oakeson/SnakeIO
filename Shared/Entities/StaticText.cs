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
                Rectangle rectangle,
                float scale = 1.0f)
        {
            Entity textInput = new Entity();
            textInput.Add(new Shared.Components.Appearance("Fonts/Micro5", typeof(SpriteFont), color, stroke, rectangle));
            textInput.Add(new Shared.Components.Readable(font, "Fonts/Micro5", value.ToString(), color, stroke, rectangle, scale));
            textInput.Add(new Shared.Components.Positionable(new Vector2(rectangle.X, rectangle.Y), 0f));

            return textInput;
        }
    }
}
