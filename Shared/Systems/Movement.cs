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
            MovePosition(entity, elapsedTime);
            // if it has camera, update camera center
            if (entity.ContainsComponent<Shared.Components.Camera>())
            {
                MoveCamera(entity);
            }
            // If Collidable update the hitbox position
            if (entity.ContainsComponent<Shared.Components.Collidable>())
            {
                MoveHitBox(entity);
            }
        }

        private void MoveCamera(Shared.Entities.Entity entity)
        {
            Shared.Components.Positionable positionable = entity.GetComponent<Shared.Components.Positionable>();
            Shared.Components.Camera camera = entity.GetComponent<Shared.Components.Camera>();
            camera.rectangle = new Rectangle((int)positionable.pos.X, (int)positionable.pos.Y, camera.rectangle.Width, camera.rectangle.Height);
            camera.center = new Point((int)positionable.pos.X, (int)positionable.pos.Y);
            camera.Follow(entity);
        }

        private void MoveHitBox(Shared.Entities.Entity entity)
        {
            Shared.Components.Positionable positionable = entity.GetComponent<Shared.Components.Positionable>();
            Shared.Components.Collidable col = entity.GetComponent<Shared.Components.Collidable>();
            if (col.Data.Shape == Shared.Components.CollidableShape.Circle)
            {
                col.Data.CircleData = new CircleData { x = positionable.pos.X, y = positionable.pos.Y, radius = col.Data.CircleData.radius };
            }
            if (col.Data.Shape == Shared.Components.CollidableShape.Rectangle)
            {
                col.Data.RectangleData = new RectangleData { x = positionable.pos.X, y = positionable.pos.Y, width = col.Data.RectangleData.width, height = col.Data.RectangleData.height };
            }
        }

        private void MovePosition(Shared.Entities.Entity entity, TimeSpan elapsedTime)
        {
            Shared.Components.Movable movable = entity.GetComponent<Shared.Components.Movable>();
            Shared.Components.Positionable positionable = entity.GetComponent<Shared.Components.Positionable>();
            // If entity is not the head don't need to update it
            if (entity.ContainsComponent<Shared.Components.Linkable>())
            {
                if (entity.GetComponent<Shared.Components.Linkable>().linkPos != LinkPosition.Head)
                {
                    return;
                }
            }
            // Don't have to update if velocity is 0
            if (movable.velocity == new Vector2(0, 0))
            {
                return;
            }
            // Cap velocity
            movable.velocity = new Vector2(Math.Clamp(movable.velocity.X, -1, 1), Math.Clamp(movable.velocity.Y, -1, 1));
            movable.velocity = (movable.velocity / movable.velocity.Length()) * .35f; // multiple by .5f for speed, and some other oddities

            Vector2 newpos = (movable.velocity) * elapsedTime.Milliseconds + positionable.pos;
            positionable.UpdatePosition(newpos);
            if (movable.velocity.X < 0 && movable.velocity.Y < 0)
            {
                positionable.orientation = (float)Math.Atan(-movable.velocity.Y / -movable.velocity.X);
                positionable.orientation = 180 + positionable.orientation;
            }
            else if (movable.velocity.X < 0)
            {
                positionable.orientation = (float)Math.Atan(movable.velocity.Y / -movable.velocity.X);
                positionable.orientation = 180 - positionable.orientation;
            }
            else
            {
                positionable.orientation = (float)Math.Atan(movable.velocity.Y / movable.velocity.X);
            }
        }
    }
}
