using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Shared
{
    [DataContract(Name ="HighScores")]
    public class HighScores
    {
        public HighScores() { }

        public HighScores(List<ulong> scores) { }


        [DataMember]
        public List<ulong>? highScores {  get; set; }

    }
}
