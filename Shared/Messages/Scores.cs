using System.Text;

namespace Shared.Messages
{
    public class Scores : Message
    {
        public Scores((string, float)[] scores) : base(Type.Scores)
        {
            this.scores = scores;
        }

        public Scores() : base(Type.Scores)
        {
            this.scores = new (string, float)[0];
        }

        public (string, float)[] scores;

        /// <summary>
        /// </summary>
        public override byte[] serialize()
        {
            List<byte> data = new List<byte>();
            data.AddRange(base.serialize());
            data.AddRange(BitConverter.GetBytes(scores.Length));
            Console.WriteLine(scores.Length);
            foreach ((string name, float score) in scores)
            {
                data.AddRange(BitConverter.GetBytes(name.Length));
                data.AddRange(Encoding.UTF8.GetBytes(name));
                data.AddRange(BitConverter.GetBytes(score));
            }

            return data.ToArray();
        }

        public override int parse(byte[] data)
        {
            int offset = base.parse(data);

            int scoreCount = BitConverter.ToInt32(data, offset);
            offset += sizeof(UInt32);

            (string, float)[] parsedScores = new (string, float)[scoreCount];
            for (int i = 0; i < scoreCount; i++)
            {
                int nameSize = BitConverter.ToInt32(data, offset);
                offset += sizeof(int);
                string name = Encoding.UTF8.GetString(data, offset, nameSize);
                offset += nameSize;
                float score = BitConverter.ToSingle(data, offset);
                offset += sizeof(Single);
                parsedScores[i] = (name, score);
            }

            this.scores = parsedScores;

            return offset;
        }
    }
}
