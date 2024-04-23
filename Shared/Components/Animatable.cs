using System;
using Microsoft.Xna.Framework.Graphics;

namespace Shared.Components
{
    public class Animatable : Component
    {
        public Texture2D spriteSheet;
        public int[] spriteTime;
        public int subImageWidth;
        public int subImageIndex { get; set; }
        public TimeSpan timeSinceLastFrame { get; set; } = TimeSpan.Zero;

        public Animatable(int[] spriteTime, Texture2D spriteSheet = null)
        {
            this.spriteSheet = spriteSheet;
            this.spriteTime = spriteTime;
            if (spriteSheet != null)
            {
                this.subImageWidth = spriteSheet.Width / spriteTime.Length;
                this.subImageIndex = 0;
            }
        }

        public override void Serialize(ref List<byte> data)
        {
            data.AddRange(BitConverter.GetBytes(spriteTime.Length));
            foreach (int time in spriteTime)
            {
                data.AddRange(BitConverter.GetBytes(time));
            }
        }
    }
}
