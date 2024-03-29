using Microsoft.Xna.Framework;

namespace Systems
{
    public class Audio : System
    {

        public Audio()
            : base(
                    typeof(Components.Audible)
                    )
        {
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
