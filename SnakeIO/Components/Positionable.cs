using Microsoft.Xna.Framework;

namespace Components
{
    /// <summary>
    /// This component is responsible for managing the state of Positionable entities.
    /// </summary>
    public class Positionable : Component
    {
        public Vector2 pos { get; set; }
        public Vector2 prevPos { get; set; }

        public Positionable(Vector2 pos)
        {
            this.pos = pos;
            this.prevPos = pos;
        }
    }
}
