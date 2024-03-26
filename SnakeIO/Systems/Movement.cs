using Microsoft.Xna.Framework;
using System;

namespace Systems
{
    /// <summary>
    /// This system is responsible for handling the movement of any
    /// entity with a movable & position components.
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

        public void MoveEntity(Entities.Entity entity, GameTime gameTime)
        {
            Components.Movable movable = entity.GetComponent<Components.Movable>();
            Components.Positionable positionable = entity.GetComponent<Components.Positionable>();
            movable.velocity *= new Vector2(.85f, .85f);

            Vector2 newPos = movable.facing * (movable.velocity * gameTime.ElapsedGameTime.Milliseconds) + positionable.pos;
            positionable.pos = newPos;
        }
    }
}
