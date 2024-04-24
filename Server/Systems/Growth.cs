using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Systems
{
    public class Growth : Shared.Systems.System
    {
        Server.GameModel.AddDelegate addDelegate;
        SortedList<Shared.Components.Growable, Shared.Entities.Entity> highScores;
        public Growth(Server.GameModel.AddDelegate addDelegate)
            : base(
                    typeof(Shared.Components.Growable),
                    typeof(Shared.Components.Linkable),
                    typeof(Shared.Components.SnakeID)
                    )
        {
            this.addDelegate = addDelegate;
            this.highScores = new SortedList<Shared.Components.Growable, Shared.Entities.Entity>();
        }

        public override void Update(TimeSpan elapsedTime)
        {
            foreach (var entity in entities.Values)
            {
                Shared.Components.Growable growable = entity.GetComponent<Shared.Components.Growable>();
                if (!highScores.ContainsKey(growable))
                {
                    highScores.Add(growable, entity);
                }

                if (growable.growth != 0 && growable.growth % 2 == 0 && growable.growth != growable.prevGrowth)
                {
                    Shared.Components.SnakeID snakeID = entity.GetComponent<Shared.Components.SnakeID>();
                    Shared.Components.Linkable linkable = entity.GetComponent<Shared.Components.Linkable>();
                    Rectangle rect = new Rectangle(0, 0, 50, 50);
                    Shared.Entities.Entity body = Shared.Entities.Body.Create("Images/body", Color.White, rect, $"{snakeID.id}", Shared.Components.LinkPosition.Body);
                    addDelegate(body);
                    Server.MessageQueueServer.instance.broadcastMessage(new Shared.Messages.NewEntity(body));
                }
                growable.prevGrowth = growable.growth;
            }
        }
    }
}
