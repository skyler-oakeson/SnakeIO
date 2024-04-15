using Microsoft.Xna.Framework;

namespace Shared.Components
{
    /// <summary>
    /// This component is responsible for keeping track of the state of movable entities.
    /// </summary>
    public class Movable : Component
    {
        public Vector2 velocity { get; set; }

        public Movable(Vector2 velocity)
        {
            this.velocity = velocity;
        }

        public override void Serialize(ref List<byte> data) {
            data.AddRange(BitConverter.GetBytes(velocity.X));
            data.AddRange(BitConverter.GetBytes(velocity.Y));
        }

    }
}
