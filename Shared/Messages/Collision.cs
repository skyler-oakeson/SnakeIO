namespace Shared.Messages
{
    public class Collision : Message
    {
        public Collision() : base(Type.Collision)
        {
        }

        public Collision(Shared.Entities.Entity e1, Shared.Entities.Entity e2) : base(Type.Collision)
        {
            if (e1.ContainsComponent<Shared.Components.Sound>())
            {
                e1Sound = e1.GetComponent<Shared.Components.Sound>();
            }

            if (e2.ContainsComponent<Shared.Components.Sound>())
            {
                e2Sound = e1.GetComponent<Shared.Components.Sound>();
            }
        }

        // e1 Sound
        public bool e1HasSound { get; private set; }
        public Components.Sound? e1Sound { get; private set; } = null;
        public Parsers.SoundParser.SoundMessage e1SoundMessage { get; private set; }

        // e2 Sound
        public bool e2HasSound { get; private set; }
        public Components.Sound? e2Sound { get; private set; } = null;
        public Parsers.SoundParser.SoundMessage e2SoundMessage { get; private set; }

        /// <summary>
        /// Just knowing this message is sent is enough
        /// </summary>
        public override byte[] serialize()
        {
            List<byte> data = new List<byte>();
            data.AddRange(base.serialize());

            data.AddRange(BitConverter.GetBytes(e1Sound != null));
            if (e1Sound != null)
            {
                e1Sound.Serialize(ref data);
            }

            data.AddRange(BitConverter.GetBytes(e2Sound != null));
            if (e2Sound != null)
            {
                e2Sound.Serialize(ref data);
            }
            return data.ToArray();
        }

        public override int parse(byte[] data)
        {
            int offset = base.parse(data);


            // e1 Sound
            this.e1HasSound = BitConverter.ToBoolean(data, offset);
            offset += sizeof(bool);
            if (e1HasSound)
            {
                Parsers.SoundParser parser = new Parsers.SoundParser();
                parser.Parse(ref data, ref offset);
                this.e1SoundMessage = parser.GetMessage();
            }

            // e2 Sound
            this.e2HasSound = BitConverter.ToBoolean(data, offset);
            offset += sizeof(bool);
            if (e2HasSound)
            {
                Parsers.SoundParser parser = new Parsers.SoundParser();
                parser.Parse(ref data, ref offset);
                this.e2SoundMessage = parser.GetMessage();
            }

            return offset;
        }
    }
}
