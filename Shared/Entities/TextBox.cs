using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Shared.Entities
{
    public class TextBox 
    {
        public static Entity Create(
                SpriteFont font,
                string value,
                Rectangle rectangle)
        {
            Entity textInput = new Entity();
            textInput.Add(new Shared.Components.Appearance("Fonts/Micro5", typeof(SpriteFont), Color.White, Color.White, rectangle));
            textInput.Add(new Shared.Components.Readable(font, "Fonts/Micro5", value.ToString(), Color.Orange, Color.Black, rectangle));
            textInput.Add(new Shared.Components.Positionable(new Vector2(rectangle.X, rectangle.Y), 0f));

            return textInput;
        }
    }
}
