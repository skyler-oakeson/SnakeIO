using System;
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
            sb.GraphicsDevice.Clear(Color.Black);
            foreach (var entity in entities.Values)
            {
                
                RenderEntity(entity);
            }
        }

        private void RenderEntity(Entities.Entity entity)
        {
            Components.Positionable positionable = entity.GetComponent<Components.Positionable>();
            Components.Renderable renderable = entity.GetComponent<Components.Renderable>();
            sb.Begin();
            sb.Draw(
                    renderable.Texture,
                    new Rectangle(
                        (int)positionable.Pos.X,
                        (int)positionable.Pos.Y,
                        renderable.Texture.Height,
                        renderable.Texture.Width
                        ),
                    renderable.color
                    );
            sb.End();
        }
    }
}
