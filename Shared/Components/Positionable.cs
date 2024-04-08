using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shared.Components
{
    /// <summary>
    /// This component is responsible for managing the state of Positionable entities.
    /// </summary>
    public class Positionable : Component
    {
        private Vector2 pos;
        public Vector2 Pos
        {
            get { return pos; }
            set { pos = value; }
        }

        public Vector2 PrevPos { get; set; }

        public Positionable(Vector2 pos)
        {
            this.pos = pos;
            this.PrevPos = pos;
        }

        public override void Serialize(ref List<byte> data)
        {
            data.AddRange(BitConverter.GetBytes(pos.X));
            data.AddRange(BitConverter.GetBytes(pos.Y));
        }

    }
}
