using Microsoft.Xna.Framework;
using System;
using Shared.Components;

namespace Shared.Systems
{
    /// <summary>
    /// This system is responsible for handling the movement of any
    /// entity with a Movable & positionable components.
    /// </summary>
    public class Movement : Shared.Systems.System
    {
        public Movement()
            : base(
                    typeof(Shared.Components.Movable),
                    typeof(Shared.Components.Positionable)
                  )
        {
        }

        public override void Update(TimeSpan elapsedTime)
        {
            foreach (var entity in entities.Values)
            {
                MoveEntity(entity, elapsedTime);
            }
        }

        private void MoveEntity(Shared.Entities.Entity entity, TimeSpan elapsedTime)
        {
            Shared.Components.Movable movable = entity.GetComponent<Shared.Components.Movable>();
            Shared.Components.Positionable positionable = entity.GetComponent<Shared.Components.Positionable>();

            // Don't have to update if velocity is 0
            if (movable.velocity.X == 0 && movable.velocity.Y == 0)
            {
                return;
            }

            // Cap velocity
            if (Math.Abs(movable.velocity.X) > 1) { movable.velocity = new Vector2(movable.velocity.X > 0 ? 1 : -1, movable.velocity.Y); }
            if (Math.Abs(movable.velocity.Y) > 1) { movable.velocity = new Vector2(movable.velocity.X, movable.velocity.Y > 0 ? 1 : -1); }

            Vector2 newpos = movable.velocity * elapsedTime.Milliseconds + positionable.pos;
            positionable.prevPos = positionable.pos;
            positionable.pos = newpos;

            // if it has camera, update camera center
            if (entity.ContainsComponent<Shared.Components.Camera>())
            {
                Shared.Components.Camera camera = entity.GetComponent<Shared.Components.Camera>();
                camera.rectangle = new Rectangle((int)positionable.pos.X, (int)positionable.pos.Y, camera.rectangle.Width, camera.rectangle.Height);
                camera.center = new Point((int)positionable.pos.X, (int)positionable.pos.Y);
                camera.Follow(entity);
            }

            movable.velocity *= new Vector2(.80f, .80f);
            positionable.orientation = (float)Math.Atan(movable.velocity.Y / movable.velocity.X);

            // If Collidable update the hitbox position
            if (entity.ContainsComponent<Shared.Components.Collidable>())
            {
                Shared.Components.Collidable col = entity.GetComponent<Shared.Components.Collidable>();
                col.hitBox = new Vector3(newpos.X, newpos.Y, col.hitBox.Z);
            }
        }
    }
}
