using Microsoft.Xna.Framework;
using System.Text;

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

        public override void Serialize(ref List<byte> data)
        {
            data.AddRange(BitConverter.GetBytes(texturePath.Length));
            data.AddRange(Encoding.UTF8.GetBytes(texturePath));
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

