using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Components
{
    class Renderable : Component
    {
        public Texture2D texture;
        public Color color;
        public Color stroke;

        public Renderable(Texture2D texture, Color color, Color stroke)
        {
            this.texture = texture;
            this.color = color;
            this.stroke = stroke;
        }
    }
}
