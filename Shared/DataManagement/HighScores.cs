using System.Runtime.Serialization;

namespace Shared.HighScores
{
    [DataContract(Name = "HighScore")]
    public class HighScore
    {
        [DataMember()]
        public int score { get; private set; }

        public HighScore(int score)
        {
            this.score = score; 
        }
    }
}
