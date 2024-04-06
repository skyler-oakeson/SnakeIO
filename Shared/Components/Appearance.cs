using Microsoft.Xna.Framework;
namespace Shared.Components
{
    public class Appearance : Component
    {
        public Appearance(string texturePath, Color color, Color stroke)
        {
            this.texturePath = texturePath;
            this.color = color;
            this.stroke = stroke;
        }

        public string texturePath { get; private set; }
        public Color color { get; private set; }
        public Color stroke { get; private set; }
    }
}

