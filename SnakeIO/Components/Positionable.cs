using Microsoft.Xna.Framework;

namespace Components
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
            set { PrevPos = pos; pos = value; }
        }

        public Vector2 PrevPos { get; private set; }

        public Positionable(Vector2 pos)
        {
            this.pos = pos;
            this.PrevPos = pos;
        }
    }
}
