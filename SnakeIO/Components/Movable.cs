using System;
using Microsoft.Xna.Framework;

namespace Components
{
    public class Movable : Component
    {
        public Vector2 facing;
        public Vector2 velocity;

        public Movable(Vector2 facing, Vector2 velocity)
        {
            this.facing = facing;
            this.velocity = velocity;
        }

    }
}
