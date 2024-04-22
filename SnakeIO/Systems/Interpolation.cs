using Microsoft.Xna.Framework;
using System;
using Shared.Entities;

namespace Systems
{
    public class Interpolation : Shared.Systems.System
    {
        public Interpolation() :
            base(
                typeof(Shared.Components.Positionable),
                typeof(Shared.Components.Movable)
                )
        {
        }

        /// <summary>
        /// Interested in entities that have both Movement and Position components,
        /// but not if they have an Input component.  Furthermore, this
        /// system adds an Goal component in order to properly update the
        /// entity's state during the update stage.
        /// </summary>
        public override bool Add(Entity entity)
        {
            bool interested = false;
            if (!entity.ContainsComponent<Shared.Components.KeyboardControllable>())
            {
                if (base.Add(entity))
                {
                    interested = true;
                    var position = entity.GetComponent<Shared.Components.Positionable>();
                    entity.Add(new Shared.Components.Interpretable(position.pos, position.orientation));
                }
            }

            return interested;
        }

        public override void Update(TimeSpan elapsedTime)
        {
            foreach (var entity in entities.Values)
            {
                var positionable = entity.GetComponent<Shared.Components.Positionable>();
                var interpretable = entity.GetComponent<Shared.Components.Interpretable>();

                if (interpretable.updateWindow > TimeSpan.Zero && interpretable.updatedTime < interpretable.updateWindow)
                {
                    interpretable.updatedTime += elapsedTime;
                    var updateFraction = (float)elapsedTime.Milliseconds / interpretable.updateWindow.Milliseconds;

                    // Turn first
                    positionable.orientation = positionable.orientation - (interpretable.startOrientation - interpretable.endOrientation) * updateFraction;

                    // Then move
                    positionable.pos = new Vector2(
                        positionable.pos.X - (interpretable.startPosition.X - interpretable.endPosition.X) * updateFraction,
                        positionable.pos.Y - (interpretable.startPosition.Y - interpretable.endPosition.Y) * updateFraction);
                }
            }
        }
    }
}
