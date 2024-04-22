using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using System.Diagnostics;

namespace Shared.Components
{
    /// <summary>
    /// This component is responsible for keeping rendering data of
    /// entites that are renderable on the screen.
    /// </summary>
    public class Readable : Component 
    {
        public Readable(string text, Color color, Color stroke, Rectangle rectangle, SpriteFont? font = null, string? fontPath = null)
        {
            Debug.Assert(font != null || fontPath != null, "Must have a font or font path.");

            this.text = text;
            this.rectangle = rectangle;
            this.color = color;
            this.stroke = stroke;
            this.fontPath = fontPath;
            this.font = font;
        }

        public string text { get; set; }
        public Rectangle rectangle { get; set; }
        public Color color { get; set; }
        public Color stroke { get; set; }
        public string? fontPath { get; set; }
        public SpriteFont? font { get; set; }

        // This will be handled in CreateEntity. These are just used as flags
        public override void Serialize(ref List<byte> data)
        {
            Debug.Assert(fontPath != null, "Must have a font path to serialize.");

            data.AddRange(BitConverter.GetBytes(fontPath.Length));
            data.AddRange(Encoding.UTF8.GetBytes(fontPath));
            data.AddRange(BitConverter.GetBytes(text.Length));
            data.AddRange(Encoding.UTF8.GetBytes(text));
            data.AddRange(BitConverter.GetBytes(rectangle.X));
            data.AddRange(BitConverter.GetBytes(rectangle.Y));
            data.AddRange(BitConverter.GetBytes(rectangle.Width));
            data.AddRange(BitConverter.GetBytes(rectangle.Height));
            data.AddRange(BitConverter.GetBytes((int)color.R));
            data.AddRange(BitConverter.GetBytes((int)color.G));
            data.AddRange(BitConverter.GetBytes((int)color.B));
            data.AddRange(BitConverter.GetBytes((int)color.A));
            data.AddRange(BitConverter.GetBytes((int)stroke.R));
            data.AddRange(BitConverter.GetBytes((int)stroke.G));
            data.AddRange(BitConverter.GetBytes((int)stroke.B));
            data.AddRange(BitConverter.GetBytes((int)stroke.A));
        }
    }
}
