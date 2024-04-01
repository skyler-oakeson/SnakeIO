using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Systems
{
    class Renderer : System
    {
        public SpriteBatch sb;
        public VertexPositionColor[] vertCircleStrip;
        public int[] indexCircleStrip;
        public BasicEffect effect;

        public Renderer(SpriteBatch sb)
            : base(
                    typeof(Components.Renderable),
                    typeof(Components.Positionable))
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
                    RenderHitbox(entity);
                }
                else
                {
                    RenderEntity(entity);
                    RenderHitbox(entity);
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

        private void RenderAnimatable(Entities.Entity entity)
        {
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

        private void RenderHitbox(Entities.Entity entity)
        {
            Components.Collidable collidable = entity.GetComponent<Components.Collidable>();
            Components.Positionable positionable = entity.GetComponent<Components.Positionable>();
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
