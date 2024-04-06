using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Systems
{
    class Renderer : Shared.Systems.System
    {
        public SpriteBatch sb;
        public VertexPositionColor[] vertCircleStrip;
        public int[] indexCircleStrip;
        public BasicEffect effect;

        public Renderer(SpriteBatch sb)
            : base(
                    typeof(Shared.Components.Renderable),
                    typeof(Shared.Components.Positionable))
        {
            this.sb = sb;

            this.effect = new BasicEffect(sb.GraphicsDevice)
            {
                VertexColorEnabled = true,
                View = Matrix.CreateLookAt(new Vector3(0, 0, 1), Vector3.Zero, Vector3.Up),

                Projection = Matrix.CreateOrthographicOffCenter(
                                           0, sb.GraphicsDevice.Viewport.Width,
                                           sb.GraphicsDevice.Viewport.Height, 0,   // doing this to get it to match the default of upper left of (0, 0)
                                           0.1f, 2)
            };

        }

        public override void Update(TimeSpan elapsedTime)
        {
            sb.GraphicsDevice.Clear(Color.Black);
            foreach (var entity in entities.Values)
            {
                if (entity.ContainsComponent<Shared.Components.Animatable>())
                {
                    Shared.Components.Animatable animatable = entity.GetComponent<Shared.Components.Animatable>();
                    animatable.timeSinceLastFrame += elapsedTime;
                    if (animatable.timeSinceLastFrame > TimeSpan.FromMilliseconds(animatable.spriteTime[animatable.subImageIndex]))
                    {
                        animatable.timeSinceLastFrame -= TimeSpan.FromMilliseconds(animatable.spriteTime[animatable.subImageIndex]);
                        animatable.subImageIndex++;
                        animatable.subImageIndex = animatable.subImageIndex % animatable.spriteTime.Length;
                    }
                    RenderAnimatable(entity);
                    RenderHitbox(entity);
                }
                else
                {
                    RenderEntity(entity);
                    RenderHitbox(entity);
                }
            }
        }

        private void RenderEntity(Shared.Entities.Entity entity)
        {
            Shared.Components.Positionable positionable = entity.GetComponent<Shared.Components.Positionable>();
            Shared.Components.Renderable renderable = entity.GetComponent<Shared.Components.Renderable>();
            sb.Begin();
            sb.Draw(
                    renderable.Texture,
                    new Rectangle(
                        (int)(positionable.Pos.X - renderable.Texture.Width/2),
                        (int)(positionable.Pos.Y - renderable.Texture.Height/2),
                        renderable.Texture.Height,
                        renderable.Texture.Width
                        ),
                    renderable.color
                    );
            sb.End();
        }

        private void RenderAnimatable(Shared.Entities.Entity entity)
        {
            Shared.Components.Positionable positionable = entity.GetComponent<Shared.Components.Positionable>();
            Shared.Components.Renderable renderable = entity.GetComponent<Shared.Components.Renderable>();
            Shared.Components.Animatable animatable = entity.GetComponent<Shared.Components.Animatable>();
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

        private void RenderHitbox(Shared.Entities.Entity entity)
        {
            Shared.Components.Positionable positionable = entity.GetComponent<Shared.Components.Positionable>();
            Shared.Components.Collidable collidable = entity.GetComponent<Shared.Components.Collidable>();
            indexCircleStrip = new int[360];
            vertCircleStrip = new VertexPositionColor[360];
            for (int i = 0; i < 360; i++)
            {
                indexCircleStrip[i] = i;
                vertCircleStrip[i].Position = new Vector3(Convert.ToSingle(positionable.Pos.X + (collidable.HitBox.Z * Math.Cos((float)i / 180 * Math.PI))), Convert.ToSingle(positionable.Pos.Y + (collidable.HitBox.Z * Math.Sin((float)i / 180 * Math.PI))), 0);
                vertCircleStrip[i].Color = Color.Red;
            }
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                sb.GraphicsDevice.DrawUserIndexedPrimitives(
                        PrimitiveType.LineStrip,
                        vertCircleStrip, 0, vertCircleStrip.Length - 1,
                        indexCircleStrip, 0, indexCircleStrip.Length - 1
                        );
            }

        }
    }
}
