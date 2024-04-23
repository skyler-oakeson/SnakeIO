using Microsoft.Xna.Framework;
using System;

namespace Shared.Components
{
    public class Interpretable : Shared.Components.Component
    {
        public Interpretable(Vector2 position, float orientation)
        {
            startPosition = position;
            endPosition = position;
            startOrientation = orientation;
            endOrientation = orientation;
        }

        public Vector2 startPosition {  get; set; }
        public Vector2 endPosition { get; set; }
        public float startOrientation { get; set; }
        public float endOrientation { get; set; }
        public TimeSpan updateWindow {  get; set; }
        public TimeSpan updatedTime { get; set; }

        public override void Serialize(ref List<byte> data) { }
    }
}
