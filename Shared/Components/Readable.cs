using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shared.Components
{
    /// <summary>
    /// This component is responsible for keeping rendering data of
    /// entites that are renderable on the screen.
    /// </summary>
    public class Readable : Appearance
    {
        public SpriteFont font { get; set; }
        public string text { get; set; }

        public Readable(SpriteFont font, string texturePath, string text, Color color, Color stroke, Rectangle rectangle): base(texturePath, typeof(SpriteFont), color, stroke, rectangle)
        {
            this.font = font;
            this.text = text;
        }

        // This will be handled in CreateEntity. These are just used as flags
        public override void Serialize(ref List<byte> data)
        {
        }
    }
}
