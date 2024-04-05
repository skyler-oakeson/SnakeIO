using Microsoft.Xna.Framework;
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

        public override void Update(GameTime gameTime)
        {
            foreach (var entity in entities.Values)
            {
                var position = entity.GetComponent<Shared.Components.Positionable>();
                
                //TODO: Implement interpolation

                // var goal = entity.get<Components.Goal>();

                // if (goal.updateWindow > TimeSpan.Zero && goal.updatedTime < goal.updateWindow)
                // {
                //     goal.updatedTime += elapsedTime;
                //     var updateFraction = (float)elapsedTime.Milliseconds / goal.updateWindow.Milliseconds;
                //
                //     // Turn first
                //     position.orientation = position.orientation - (goal.startOrientation - goal.goalOrientation) * updateFraction;
                //
                //     // Then move
                //     position.position = new Vector2(
                //         position.position.X - (goal.startPosition.X - goal.goalPosition.X) * updateFraction,
                //         position.position.Y - (goal.startPosition.Y - goal.goalPosition.Y) * updateFraction);
                // }
            }
        }
    }
}
