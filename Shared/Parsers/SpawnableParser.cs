using System.Text;
namespace Shared.Parsers
{
    public class SpawnableParser : Parser
    {
        private SpawnableMessage message { get; set; }
        public SpawnableMessage Message
        {
            get { return message; }
            set { message = value; }
        }

        public override void Parse(ref byte[] data, ref int offset)
        {
            int rate = BitConverter.ToInt32(data, offset);
            TimeSpan messageSpawnRate = new TimeSpan(rate);
            offset += sizeof(Int32);
            int messageSpawnCount = BitConverter.ToInt32(data, offset);
            offset += sizeof(Int32);
            int typeSize = BitConverter.ToInt32(data, offset);
            offset += sizeof(Int32);
            System.Type messageType = System.Type.GetType(Encoding.UTF8.GetString(data, offset, typeSize));
            offset += typeSize;
            this.message = new SpawnableMessage()
            {
                spawnRate = messageSpawnRate,
                spawnCount = messageSpawnCount,
                type = messageType
            };
        }

        public SpawnableMessage GetMessage()
        {
            return Message;
        }

        public struct SpawnableMessage
        {
            public TimeSpan spawnRate { get; set; }
            public int spawnCount { get; set; }
            public Type type { get; set; }
        }
    }
}
