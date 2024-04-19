using Microsoft.Xna.Framework;

namespace Systems
{
    public class Growth : Shared.Systems.System
    {
        Server.GameModel.AddDelegate addDelegate;
        public Growth(Server.GameModel.AddDelegate addDelegate)
            : base(
                    typeof(Shared.Components.Growable),
                    typeof(Shared.Components.Linkable)
                    )
        {
            this.addDelegate = addDelegate;
        }

        public override void Update(TimeSpan elapsedTime)
        {
            foreach (var entity in entities.Values)
            {
                Shared.Components.Growable growable = entity.GetComponent<Shared.Components.Growable>();
                if (growable.growth != 0 && growable.growth % 5 == 0)
                {
                    Console.WriteLine(growable.growth);
                    Shared.Components.Linkable linkable = entity.GetComponent<Shared.Components.Linkable>();
                    Rectangle rect = new Rectangle(0, 0, 50, 50);
                    Shared.Entities.Entity body = Shared.Entities.Body.Create("Images/body", Color.White, "Audio/bass-switch", rect, linkable.chain, Shared.Components.LinkPosition.Body);
                    addDelegate(body);
                    Server.MessageQueueServer.instance.broadcastMessage(new Shared.Messages.NewEntity(body));
                    growable.growth = 0;
                }
            }
        }
    }
}
