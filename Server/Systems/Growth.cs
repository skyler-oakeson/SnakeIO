using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Systems
{
    public class Growth : Shared.Systems.System
    {
        Server.GameModel.AddDelegate addDelegate;
        List<Score> scores;
        public Growth(Server.GameModel.AddDelegate addDelegate)
            : base(
                    typeof(Shared.Components.Growable),
                    typeof(Shared.Components.Linkable),
                    typeof(Shared.Components.SnakeID)
                    )
        {
            this.addDelegate = addDelegate;
            this.scores = new List<Score>();
        }

        public override void Update(TimeSpan elapsedTime)
        {
            foreach (var entity in entities.Values)
            {
                Shared.Components.Growable growable = entity.GetComponent<Shared.Components.Growable>();
                if (!scores.Contains(new Score(entity.GetComponent<Shared.Components.SnakeID>().name, growable)))
                {
                    scores.Add(new Score(entity.GetComponent<Shared.Components.SnakeID>().name, growable));
                }

                if (growable.growth != 0 && growable.growth % 2 == 0 && growable.growth != growable.prevGrowth)
                {
                    Shared.Components.SnakeID snakeID = entity.GetComponent<Shared.Components.SnakeID>();
                    Shared.Components.Linkable linkable = entity.GetComponent<Shared.Components.Linkable>();
                    Color color = linkable.nextEntity.GetComponent<Shared.Components.Appearance>().color;
                    Rectangle rect = new Rectangle(0, 0, 50, 50);
                    Shared.Entities.Entity body = Shared.Entities.Body.Create("Images/body", color, rect, $"{snakeID.id}", Shared.Components.LinkPosition.Body);
                    body.Remove<Shared.Components.Invincible>();
                    addDelegate(body);
                    Server.MessageQueueServer.instance.broadcastMessage(new Shared.Messages.NewEntity(body));
                }
                growable.prevGrowth = growable.growth;
            }
            scores.Sort();

            (string, float)[] highScores = new (string, float)[scores.Count];
            for (int i = 0; i < scores.Count(); i++)
            {
                Score score = scores.ElementAt(i);
                string name = score.name;
                Shared.Components.Growable size = score.size;
                highScores[i] = (name, size.growth);
            }

            if (highScores.Count() > 0)
            {
                Server.MessageQueueServer.instance.broadcastMessage(new Shared.Messages.Scores(highScores));
            }

        }
        public class Score : IComparable<Score>, IEquatable<Score>
        {
            public Score (string name, Shared.Components.Growable size)
            {
                this.name = name;
                this.size = size;
            }

            public string name { get; set; }
            public Shared.Components.Growable size { get; set; }

            public int CompareTo(Score to)
            {
                return to.size.growth.CompareTo(size.growth);
            }

            public bool Equals(Score score)
            {
                return score.name == name;
            }
        }
    }
}
