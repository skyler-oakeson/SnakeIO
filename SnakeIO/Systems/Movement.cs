using Microsoft.Xna.Framework;
using System;

namespace Systems
{
    /// <summary>
    /// This system is responsible for handling the movement of any
    /// entity with a Movable & positionable components.
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

            // Don't have to update if velocity is 0
            if (movable.velocity.X == 0 && movable.velocity.Y == 0)
            {
                return;
            }

            // Cap velocity
            if (Math.Abs(movable.velocity.X) > 1) { movable.velocity = new Vector2(movable.velocity.X > 0 ? 1 : -1, movable.velocity.Y); }
            if (Math.Abs(movable.velocity.Y) > 1) { movable.velocity = new Vector2(movable.velocity.X, movable.velocity.Y > 0 ? 1 : -1); }

            Vector2 newpos = movable.velocity * gameTime.ElapsedGameTime.Milliseconds + positionable.pos;
            positionable.prevPos = positionable.pos;
            positionable.pos = newpos;

            movable.velocity *= new Vector2(.80f, .80f);

            // If Collidable update the hitbox position
            if (entity.ContainsComponent<Components.Collidable>())
            {
                Components.Collidable col = entity.GetComponent<Components.Collidable>();
                col.hitBox = new Vector3(newpos.X, newpos.Y, col.hitBox.Z);
            }
        }
    }
}
