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
                if (entity.ContainsComponent<Components.Animatable>())
                {
                    Components.Animatable animatable = entity.GetComponent<Components.Animatable>();
                    animatable.timeSinceLastFrame += gameTime.ElapsedGameTime;
                    if (animatable.timeSinceLastFrame > TimeSpan.FromMilliseconds(animatable.spriteTime[animatable.subImageIndex]))
                    {
                        animatable.timeSinceLastFrame -= TimeSpan.FromMilliseconds(animatable.spriteTime[animatable.subImageIndex]);
                        animatable.subImageIndex++;
                        animatable.subImageIndex = animatable.subImageIndex % animatable.spriteTime.Length;
                    }
                    RenderAnimatable(entity);
                }
                else {
                    RenderEntity(entity);
                }
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

        private void RenderAnimatable(Entities.Entity entity) {
            Components.Positionable positionable = entity.GetComponent<Components.Positionable>();
            Components.Renderable renderable = entity.GetComponent<Components.Renderable>();
            Components.Animatable animatable = entity.GetComponent<Components.Animatable>();
            sb.Begin();
            sb.Draw(
                    animatable.spriteSheet,
                    new Rectangle(
                        (int)positionable.Pos.X,
                        (int)positionable.Pos.Y,
                        animatable.subImageWidth,
                        animatable.spriteSheet.Height
                        ),
                    new Rectangle(animatable.subImageIndex * animatable.subImageWidth, 0, animatable.subImageWidth, animatable.spriteSheet.Height), // Source sub-texture
                    renderable.Color,
                    0, // Angular rotation
                    new Vector2(animatable.subImageWidth / 2, animatable.spriteSheet.Height / 2), // Center point of rotation
                    SpriteEffects.None, 0);
            sb.End();
        }
    }
}
