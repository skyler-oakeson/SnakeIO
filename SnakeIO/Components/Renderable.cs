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
        private Texture2D texture {get; set;}
        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public Color color {get; set;}
        public Color Color 
        {
            get { return color; }
            set { color = value; }
        }

        public Color stroke {get; set;}
        public Color Stroke 
        {
            get { return stroke; }
            set { stroke = value; }
        }

        public Renderable(Texture2D texture, Color color, Color stroke)
        {
            this.texture = texture;
            this.color = color;
            this.stroke = stroke;
        }
    }
}
