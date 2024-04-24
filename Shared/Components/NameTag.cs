using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shared.Components
{
    /// <summary>
    /// This component is responsible for keeping rendering data of
    /// entites that are renderable on the screen.
    /// </summary>
    public class NameTag : Component 
    {
        public SpriteFont font { get; set; }
        public string name { get; set; }

        public NameTag(SpriteFont font, string name)
        {
            this.font = font;
            this.name = name;
        }

        // This will be handled in CreateEntity. These are just used as flags
        public override void Serialize(ref List<byte> data)
        {
        }
    }
}
