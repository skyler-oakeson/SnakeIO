#nullable enable
using Microsoft.Xna.Framework;

namespace Components
{
    /// <summary>
    /// This component is responsible for keeping rendering data of
    /// entites that are renderable on the screen.
    /// </summary>
    class Renderable<T> : Component
    {
        public T texture { get; set; }
        public Color color { get; set; }
        public Color stroke { get; set; }
        public string? label { get; set; }

        public Renderable(T texture, Color color, Color stroke, string? label = null)
        {
            this.texture = texture;
            this.color = color;
            this.stroke = stroke;
            this.label = label;
        }
    }
}
