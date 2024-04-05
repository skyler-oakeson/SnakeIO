using Microsoft.Xna.Framework.Input;
namespace Shared.Messages
{
    public class Input : Message
    {
        //TODO: Research posibilities to handle Mouse controllable as well
        public Input(uint entityId, List<Shared.Controls.Control> inputs, TimeSpan elapsedTime) : base(Messages.Type.Input)
        {
            this.entityId = entityId;
            this.inputs = inputs;
            this.elapsedTime = elapsedTime;
        }

        public Input() : base(Messages.Type.Input)
        {
            this.elapsedTime = TimeSpan.Zero;
            this.inputs = new List<Shared.Controls.Control>();
        }

        public uint entityId { get; private set; }
        public List<Shared.Controls.Control> inputs { get; private set; }
        public TimeSpan elapsedTime { get; private set; }

        public override byte[] serialize()
        {
            List<byte> data = new List<byte>();
            data.AddRange(base.serialize());
            data.AddRange(BitConverter.GetBytes(entityId));
            
            data.AddRange(BitConverter.GetBytes(inputs.Count));
            foreach (Shared.Controls.Control con in inputs)
            {
                data.AddRange(BitConverter.GetBytes((UInt16)con.sc));
                data.AddRange(BitConverter.GetBytes((UInt16)con.cc));
                if (con.key != null)
                {
                    data.AddRange(BitConverter.GetBytes((UInt16)con.key));
                }
                data.AddRange(BitConverter.GetBytes(con.keyPressOnly));
            }
            data.AddRange(BitConverter.GetBytes(elapsedTime.Milliseconds));

            return data.ToArray();
        }

        public override int parse(byte[] data)
        {
            int offset = base.parse(data);

            this.entityId = BitConverter.ToUInt32(data, offset);
            offset += sizeof(UInt32);

            int inputCount = BitConverter.ToInt32(data, offset);
            offset += sizeof(UInt32);
            for (int i = 0; i < inputCount; i++)
            {
                Scenes.SceneContext sc = (Scenes.SceneContext)BitConverter.ToUInt16(data, offset); //scene context comes first
                offset += sizeof(UInt16);
                Shared.Controls.ControlContext cc = (Shared.Controls.ControlContext)BitConverter.ToUInt16(data, offset); //control context comes second
                offset += sizeof(UInt16);
                Keys key = (Keys) BitConverter.ToUInt16(data, offset);
                offset += sizeof(UInt16);
                bool keyPressOnly = BitConverter.ToBoolean(data, offset);
                offset += sizeof(bool);
                inputs.Add(new Shared.Controls.Control(sc, cc, key, null, keyPressOnly));
            }

            elapsedTime = new TimeSpan( 0, 0, 0, 0, BitConverter.ToInt32(data, offset));
            offset += sizeof(Int32);

            return offset;
        }
    }
}
