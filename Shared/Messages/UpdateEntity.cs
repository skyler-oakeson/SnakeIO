
using Microsoft.Xna.Framework;
using Shared.Components;
using Shared.Entities;

namespace Shared.Messages
{
    public class UpdateEntity : Message
    {
        public UpdateEntity(Entity entity, TimeSpan updateWindow) : base(Type.UpdateEntity)
        {
            this.id = entity.id;

            if (entity.ContainsComponent<Positionable>() && entity.ContainsComponent<Growable>())
            {
                this.hasPosition = true;
                this.position = entity.GetComponent<Positionable>().pos;
                this.prevPosition = entity.GetComponent<Positionable>().prevPos;
                this.growth = entity.GetComponent<Growable>().growth;
            }
            if (entity.ContainsComponent<ParticleComponent>())
            {
                particleComponent = entity.GetComponent<ParticleComponent>();
            }

            this.updateWindow = updateWindow;
        }

        public UpdateEntity() : base(Type.UpdateEntity)
        {
        }

        public uint id { get; private set; }

        // Position
        public bool hasPosition { get; private set; } = false;
        public Vector2 position { get; private set; }
        public Vector2 prevPosition { get; private set; }
        public float orientation { get; private set; }
        public float growth { get; private set; }

        // Particle
        // Particle
        public bool hasParticle { get; private set; }
        public Components.ParticleComponent? particleComponent { get; private set; } = null;
        public Parsers.ParticleParser.ParticleMessage particleMessage { get; private set; }

        // Only the milliseconds are used/serialized
        public TimeSpan updateWindow { get; private set; } = TimeSpan.Zero;

        public override byte[] serialize()
        {
            List<byte> data = new List<byte>();

            data.AddRange(base.serialize());
            data.AddRange(BitConverter.GetBytes(id));

            data.AddRange(BitConverter.GetBytes(hasPosition));
            if (hasPosition)
            {
                data.AddRange(BitConverter.GetBytes(position.X));
                data.AddRange(BitConverter.GetBytes(position.Y));
                data.AddRange(BitConverter.GetBytes(prevPosition.X));
                data.AddRange(BitConverter.GetBytes(prevPosition.Y));
                data.AddRange(BitConverter.GetBytes(orientation));
                data.AddRange(BitConverter.GetBytes(growth));
            }

            data.AddRange(BitConverter.GetBytes(particleComponent != null));
            if (particleComponent != null)
            {
                particleComponent.Serialize(ref data);
            }

            data.AddRange(BitConverter.GetBytes(updateWindow.Milliseconds));

            return data.ToArray();
        }

        public override int parse(byte[] data)
        {
            int offset = base.parse(data);

            this.id = BitConverter.ToUInt32(data, offset);
            offset += sizeof(uint);

            this.hasPosition = BitConverter.ToBoolean(data, offset);
            offset += sizeof(bool);
            if (hasPosition)
            {
                float positionX = BitConverter.ToSingle(data, offset);
                offset += sizeof(Single);
                float positionY = BitConverter.ToSingle(data, offset);
                offset += sizeof(Single);
                float prevPositionX = BitConverter.ToSingle(data, offset);
                offset += sizeof(Single);
                float prevPositionY = BitConverter.ToSingle(data, offset);
                offset += sizeof(Single);
                this.position = new Vector2(positionX, positionY);
                this.prevPosition = new Vector2(prevPositionX, prevPositionY);
                this.orientation = BitConverter.ToSingle(data, offset);
                offset += sizeof(Single);
                this.growth = BitConverter.ToSingle(data, offset);
                offset += sizeof(Single);
            }

            this.hasParticle = BitConverter.ToBoolean(data, offset);
            offset += sizeof(bool);
            if (hasParticle)
            {
                Parsers.ParticleParser parser = new Parsers.ParticleParser();
                parser.Parse(ref data, ref offset);
                this.particleMessage = parser.GetMessage();
            }

            this.updateWindow = new TimeSpan(0, 0, 0, 0, BitConverter.ToInt32(data, offset));
            offset += sizeof(Int32);

            return offset;
        }
    }
}
