using Microsoft.Xna.Framework;
using System;
using Shared.Components;

namespace Systems
{
    /// <summary>
    /// This system is responsible for handling the movement of any
    /// entity with a Movable & Positionable components.
    /// </summary>
    class Movement : Shared.Systems.System
    {
        public Movement()
            : base(
                    typeof(Shared.Components.Movable),
                    typeof(Shared.Components.Positionable)
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

        private void MoveEntity(Shared.Entities.Entity entity, GameTime gameTime)
        {
            Shared.Components.Movable movable = entity.GetComponent<Shared.Components.Movable>();
            Shared.Components.Positionable positionable = entity.GetComponent<Shared.Components.Positionable>();

            // Don't have to update if velocity is 0
            if (movable.Velocity.X == 0 && movable.Velocity.Y == 0)
            {
                return;
            }

            // Cap velocity
            if (Math.Abs(movable.Velocity.X) > 1) { movable.Velocity = new Vector2(movable.Velocity.X > 0 ? 1 : -1, movable.Velocity.Y); }
            if (Math.Abs(movable.Velocity.Y) > 1) { movable.Velocity = new Vector2(movable.Velocity.X, movable.Velocity.Y > 0 ? 1 : -1); }

            Vector2 newPos = movable.Velocity * gameTime.ElapsedGameTime.Milliseconds + positionable.Pos;
            positionable.PrevPos = positionable.Pos;
            positionable.Pos = newPos;

            movable.Velocity *= new Vector2(.80f, .80f);

            // If Collidable update the hitbox position
            if (entity.ContainsComponent<Shared.Components.Collidable>())
            {
                Shared.Components.Collidable col = entity.GetComponent<Shared.Components.Collidable>();
                col.HitBox = new Vector3(newPos.X, newPos.Y, col.HitBox.Z);
            }
        }
    }
}
