using Microsoft.Xna.Framework;
using System;

namespace Systems
{
    /// <summary>
    /// This system is responsible for handling the movement of any
    /// entity with a Movable & Positionable components.
    /// </summary>
    class Movement : System
    {
        public Movement()
            : base(
                    typeof(Components.Movable),
                    typeof(Components.Positionable)
                  )
        {
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var entity in entities.Values)
            {
                MoveEntity(entity, gameTime);
            }
        }

        private void MoveEntity(Entities.Entity entity, GameTime gameTime)
        {
            Components.Movable movable = entity.GetComponent<Components.Movable>();
            Components.Positionable positionable = entity.GetComponent<Components.Positionable>();

            // Cap velocity
            if (Math.Abs(movable.Velocity.X) > 1) { movable.Velocity = new Vector2(movable.Velocity.X > 0 ? 1 : -1, movable.Velocity.Y); }
            if (Math.Abs(movable.Velocity.Y) > 1) { movable.Velocity = new Vector2(movable.Velocity.X, movable.Velocity.Y > 0 ? 1 : -1); }

            Vector2 newPos = movable.Velocity * gameTime.ElapsedGameTime.Milliseconds + positionable.Pos;
            positionable.Pos = newPos;

            movable.Velocity *= new Vector2(.85f, .85f);

            // If Collidable update the hitbox position
            if (entity.ContainsComponent<Components.Collidable>())
            {
                Components.Collidable col = entity.GetComponent<Components.Collidable>();
                col.HitBox = new Vector3(newPos.X, newPos.Y, col.HitBox.Z);
            }
        }
    }
}
