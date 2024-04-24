namespace Shared.Messages
{
    public class Scores : Message
    {
        public Scores(float[] scores) : base(Type.Scores)
        {
            this.scores = scores;
        }

        public Scores() : base(Type.Scores)
        {
            this.scores = new float[0];
        }

        public float[] scores;

        /// <summary>
        /// </summary>
        public override byte[] serialize()
        {
            List<byte> data = new List<byte>();
            data.AddRange(base.serialize());
            data.AddRange(BitConverter.GetBytes(scores.Length));
            foreach (float score in scores)
            {
                data.AddRange(BitConverter.GetBytes(score));
            }

            return data.ToArray();
        }

        public override int parse(byte[] data)
        {
            int offset = base.parse(data);

            int scoreCount = BitConverter.ToInt32(data, offset);
            offset += sizeof(UInt32);

            float[] parsedScores = new float[scoreCount];
            for (int i = 0; i < scoreCount; i++)
            {
                float score = BitConverter.ToSingle(data, offset);
                parsedScores[i] = score;
                offset += sizeof(Single);
            }

            this.scores = parsedScores;

            return offset;
        }
    }
}
