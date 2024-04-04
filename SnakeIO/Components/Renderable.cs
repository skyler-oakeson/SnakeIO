using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Components
{
    /// <summary>
    /// This component is responsible for keeping rendering data of
    /// entites that are renderable on the screen.
    /// </summary>
    class Renderable : Component
    {
        public Texture2D texture {get; set;}
        public Color color {get; set;}
        public Color stroke {get; set;}

        public Renderable(Texture2D texture, Color color, Color stroke)
        {
            this.texture = texture;
            this.color = color;
            this.stroke = stroke;
        }
    }
}
