using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shared.Components
{
    /// <summary>
    /// This component is responsible for keeping rendering data of
    /// entites that are renderable on the screen.
    /// </summary>
    public class Renderable : Appearance 
    {
        public Texture2D texture { get; set; }

        public Renderable(Texture2D texture, string texturePath, Color color, Color stroke): base(texturePath, typeof(Texture2D), color, stroke)
        {
            this.texture = texture;
        }

        // This will be handled in CreateEntity. These are just used as flags
        public override void Serialize(ref List<byte> data)
        {
        }
    }
}
