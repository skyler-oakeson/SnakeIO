using System;
using Microsoft.Xna.Framework.Graphics;

namespace Shared.Components
{
    public class Animatable : Component
    {
        public Texture2D spriteSheet;
        public int[] spriteTime;
        public int subImageWidth;
        public int subImageIndex { get; set;}
        public TimeSpan timeSinceLastFrame { get; set; } = TimeSpan.Zero;

        public Animatable(Texture2D spriteSheet, int[] spriteTime)
        {
            this.spriteSheet = spriteSheet;
            this.spriteTime = spriteTime;
            this.subImageWidth = spriteSheet.Width / spriteTime.Length;
            this.subImageIndex = 0;
        }

        // TODO: Parse everything except for Texture2D
        public override void Serialize(ref List<byte> data)
        {
        }
    }
}
