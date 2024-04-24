using System.Text;
using Microsoft.Xna.Framework;
using System;
namespace Shared.Parsers
{
    public class ParticleParser : Parser
    {
        private ParticleMessage message { get; set; }
        public ParticleMessage Message
        {
            get { return message; }
            set { message = value; }
        }

        public override void Parse(ref byte[] data, ref int offset)
        {
            UInt16 particleType = BitConverter.ToUInt16(data, offset);
            offset += sizeof(UInt16);
            float centerX = BitConverter.ToSingle(data, offset);
            offset += sizeof(float);
            float centerY = BitConverter.ToSingle(data, offset);
            offset += sizeof(float);
            float directionX = BitConverter.ToSingle(data, offset);
            offset += sizeof(float);
            float directionY = BitConverter.ToSingle(data, offset);
            offset += sizeof(float);
            float speed = BitConverter.ToSingle(data, offset);
            offset += sizeof(float);
            float sizeX = BitConverter.ToSingle(data, offset);
            offset += sizeof(float);
            float sizeY = BitConverter.ToSingle(data, offset);
            offset += sizeof(float);
            TimeSpan lifetime = new TimeSpan(BitConverter.ToInt32(data, offset));
            offset += sizeof(Int32);
            bool shouldCreate = BitConverter.ToBoolean(data, offset);
            offset += sizeof(bool);
            this.message = new ParticleMessage
            {
                type = (Shared.Components.ParticleComponent.ParticleType) particleType,
                center = new Vector2(centerX, centerY),
                direction = new Vector2(directionX, directionY),
                speed = speed,
                size = new Vector2(sizeX, sizeY),
                lifetime = lifetime,
                shouldCreate = shouldCreate
            };
        }

        public ParticleMessage GetMessage()
        {
            return Message;
        }

        public struct ParticleMessage
        {
            public Shared.Components.ParticleComponent.ParticleType type { get; set; }
            public Vector2 center { get; set; }
            public Vector2 direction { get; set; }
            public float speed { get; set; }
            public Vector2 size { get; set; }
            public TimeSpan lifetime { get; set; }
            public bool shouldCreate { get; set; }
        }
    }
}
