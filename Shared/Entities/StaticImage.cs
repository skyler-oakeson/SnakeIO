using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
            textInput.Add(new Shared.Components.Appearance());
            textInput.Add(new Shared.Components.Renderable(Color.White, Color.White, new Rectangle(x, y, texture.Width, texture.Height), texture: texture));
            textInput.Add(new Shared.Components.Positionable(new Vector2(x, y), 0f));

            return textInput;
        }
    }
}
