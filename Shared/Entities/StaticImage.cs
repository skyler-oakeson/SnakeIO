using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Shared.Entities
{
    public class StaticImage 
    {
        public static Entity Create(
                Texture2D texture,
                string path,
                int x,
                int y)
        {
            Entity textInput = new Entity();
            textInput.Add(new Shared.Components.Appearance("Fonts/Micro5", typeof(SpriteFont), Color.White, Color.White, new Rectangle(x, y, texture.Width, texture.Height)));
            textInput.Add(new Shared.Components.Renderable(texture, path, Color.White, Color.White, new Rectangle(x, y, texture.Width, texture.Height)));
            textInput.Add(new Shared.Components.Positionable(new Vector2(x, y), 0f));

            return textInput;
        }
    }
}
