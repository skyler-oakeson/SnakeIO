using Microsoft.Xna.Framework;

namespace Components
{
    public class Positionable : Component
    {
        public Vector2 pos {get; set;}

        public Positionable(Vector2 pos)
        {
            this.pos = pos;
        }

    }
}
