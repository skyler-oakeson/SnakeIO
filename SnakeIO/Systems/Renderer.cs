using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Systems
{
    class Renderer : System
    {
        public SpriteBatch sb;

        public Renderer(SpriteBatch sb)
            : base(
                    typeof(Components.Renderable),
                    typeof(Components.Positionable))
        {
            this.sb = sb;
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var entity in entities.Values)
            {
                RenderEntity(entity);
            }
        }

        private void RenderEntity(Entities.Entity entity)
        {
            Components.Positionable positionable = entity.GetComponent<Components.Positionable>();
            Components.Renderable renderable = entity.GetComponent<Components.Renderable>();
            sb.GraphicsDevice.Clear(Color.Black);
            sb.Begin();
            sb.Draw(
                    renderable.texture,
                    new Rectangle(
                        (int)positionable.pos.X,
                        (int)positionable.pos.Y,
                        renderable.texture.Height,
                        renderable.texture.Width
                        ),
                    renderable.color
                    );
            sb.End();
        }
    }
}
