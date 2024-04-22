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
    public class Renderable : Component 
    {
        public Renderable(Color color, Color stroke, Rectangle rectangle, Texture2D? texture = null, string? texturePath = null)
        {
            Debug.Assert(texture != null || texturePath != null, "Must have a texture or texture path");

            this.texture = texture;
            this.texturePath = texturePath;
            this.rectangle = rectangle;
            this.color = color;
            this.stroke = stroke;
        }

        public Texture2D? texture { get; set; }
        public string? texturePath { get; set; }
        public Rectangle rectangle { get; set; }
        public Color color { get; set; }
        public Color stroke { get; set; }


        // This will be handled in CreateEntity. These are just used as flags
        public override void Serialize(ref List<byte> data)
        {
            Debug.Assert(texturePath != null, "Must have a texture path to serialize");
            data.AddRange(BitConverter.GetBytes(texturePath.Length));
            data.AddRange(Encoding.UTF8.GetBytes(texturePath));
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
