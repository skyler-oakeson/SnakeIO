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

        public override void Update(TimeSpan elapsedTime)
        {
            foreach (var entity in entities.Values)
            {
                var position = entity.GetComponent<Shared.Components.Positionable>();
                
                //TODO: Implement interpolation
            }
        }
    }
}
