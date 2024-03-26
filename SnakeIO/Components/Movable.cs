using System;
using Microsoft.Xna.Framework;

namespace Components
{
    public class Movable : Component
    {
        public Vector2 facing;
        public Vector2 velocity;
        public TimeSpan lastMoved;

        public Movable(Vector2 facing, Vector2 velocity, TimeSpan lastMoved)
        {
            this.facing = facing;
            this.velocity = velocity;
            this.lastMoved = lastMoved;
        }

    }
}
