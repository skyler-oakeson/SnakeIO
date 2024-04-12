using Microsoft.Xna.Framework;
using System.Text;

namespace Shared.Components
{
    public class Appearance : Component
    {
        public Appearance(string texturePath, Type type, Color color, Color stroke)
        {
            this.texturePath = texturePath;
            this.type = type;
            this.color = color;
            this.stroke = stroke;
        }

        public string texturePath { get; set; }
        public Color color { get; set; }
        public Color stroke { get; set; }
        public Type type { get; set; }

        public override void Serialize(ref List<byte> data)
        {
            data.AddRange(BitConverter.GetBytes(texturePath.Length));
            data.AddRange(Encoding.UTF8.GetBytes(texturePath));
            data.AddRange(BitConverter.GetBytes(type.AssemblyQualifiedName.Length));
            data.AddRange(Encoding.UTF8.GetBytes(type.AssemblyQualifiedName));
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

