using Microsoft.Xna.Framework;

namespace Systems
{
    public class Growth : Shared.Systems.System
    {
        public Growth()
            : base(
                    typeof(Shared.Components.Growable)
                    )
        {
        }

        public override void Update(TimeSpan elapsedTime)
        {
            foreach (var entity in entities.Values)
            {
                Shared.Components.Growable growable = entity.GetComponent<Shared.Components.Growable>();

            }
        }
    }
}
