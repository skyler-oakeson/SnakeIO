using Microsoft.Xna.Framework;

namespace Systems
{
    /// <summary>
    /// This system is responsible for handling the collision of any
    /// entities with Collidable, & Positionable components.
    /// This system handles entites with Movable components (Not Required).
    /// </summary>
    public class Collision : Shared.Systems.System
    {
        private List<Shared.Entities.Entity> removeThese = new List<Shared.Entities.Entity>();
        private Server.GameModel.RemoveDelegate removeEntity;
        public Collision(Server.GameModel.RemoveDelegate removeEntity)
            : base(
                    typeof(Shared.Components.Collidable),
                    typeof(Shared.Components.Positionable)
                    )
        {
            this.removeEntity = removeEntity;
        }

        public override void Update(TimeSpan elapsedTime)
        {
            Shared.Entities.Entity[] entityArr = new Shared.Entities.Entity[entities.Values.Count];
            entities.Values.CopyTo(entityArr, 0);
            for (int i = 0; i < entityArr.Length; i++)
            {
                Shared.Entities.Entity e1 = entityArr[i];
                for (int j = i; j < entityArr.Length; j++)
                {
                    Shared.Entities.Entity e2 = entityArr[j];
                    // if e1 and e2 are not movable, we should not check
                    bool shouldCheck = !(!e1.ContainsComponent<Shared.Components.Movable>() && !e2.ContainsComponent<Shared.Components.Movable>());
                    if (shouldCheck)
                    {
                        bool res = DidCollide(e1, e2);
                        if (res) HandleCollision(e1, e2);
                    }
                }
            }
            foreach (Shared.Entities.Entity entity in removeThese)
            {
                removeEntity(entity);
            }
        }

        private bool DidCollide(Shared.Entities.Entity e1, Shared.Entities.Entity e2)
        {
            if (e1 == e2)
            {
                return false;
            }

            Shared.Components.Collidable e1Col = e1.GetComponent<Shared.Components.Collidable>();
            Shared.Components.Collidable e2Col = e2.GetComponent<Shared.Components.Collidable>();
            Shared.Components.Positionable e1Pos = e1.GetComponent<Shared.Components.Positionable>();
            Shared.Components.Positionable e2Pos = e2.GetComponent<Shared.Components.Positionable>();
            double hitDist = Math.Pow(e1Col.hitBox.Z + e2Col.hitBox.Z, 2);
            double dist = Math.Pow(Math.Abs(e1Pos.pos.X - e2Pos.pos.X), 2) + Math.Pow(Math.Abs(e1Pos.pos.Y - e2Pos.pos.Y), 2);

            if (dist < hitDist)
            {
                return true;
            }

            return false;
        }

        private void HandleCollision(Shared.Entities.Entity e1, Shared.Entities.Entity e2)
        {
            Shared.Components.Positionable e1Pos = e1.GetComponent<Shared.Components.Positionable>();
            Shared.Components.Positionable e2Pos = e2.GetComponent<Shared.Components.Positionable>();
            Vector2 n = (e1Pos.pos - e2Pos.pos);
            n.Normalize();

            //These two lines cause the snake to stop, so I am commenting them out unless we need them
            // e1Pos.pos = e1Pos.prevPos;
            // e2Pos.pos = e2Pos.prevPos;

            //if (e1.ContainsComponent<Shared.Components.Audible>())
            //{
            //    e1.GetComponent<Shared.Components.Audible>().Play = true;
            //}

            // Movables - Non-Movables
            // if (e1.ContainsComponent<Shared.Components.Movable>() && !e2.ContainsComponent<Shared.Components.Movable>())
            // {
            //     Shared.Components.Movable e1Mov = e1.GetComponent<Shared.Components.Movable>();
            //     e1Mov.velocity += n;
            // }

            // TODO: I don't think we are going to want to update the velo by n
            // Determine if this is needed or not
            // Movables - Movables
            if (e1.ContainsComponent<Shared.Components.Movable>() && e2.ContainsComponent<Shared.Components.Movable>())
            {
                Shared.Components.Movable e1Mov = e1.GetComponent<Shared.Components.Movable>();
                Shared.Components.Movable e2Mov = e2.GetComponent<Shared.Components.Movable>();
                // e2Mov.velocity -= n * .5f;
                // e1Mov.velocity += n * .5f;
            }

            //Consumables
            if (e1.ContainsComponent<Shared.Components.Consumable>() || e2.ContainsComponent<Shared.Components.Consumable>())
            {
                if (e1.ContainsComponent<Shared.Components.Consumable>())
                {
                    Shared.Components.Consumable consumable = e1.GetComponent<Shared.Components.Consumable>();
                    Server.MessageQueueServer.instance.broadcastMessage(new Shared.Messages.RemoveEntity(e1.id));
                    removeThese.Add(e1);
                    if (e2.ContainsComponent<Shared.Components.Growable>())
                    {
                        Shared.Components.Growable growthComponent = e2.GetComponent<Shared.Components.Growable>();
                        growthComponent.growth += consumable.growth;
                    }
                }
                else
                {
                    Shared.Components.Consumable consumable = e2.GetComponent<Shared.Components.Consumable>();
                    Server.MessageQueueServer.instance.broadcastMessage(new Shared.Messages.RemoveEntity(e2.id));
                    removeThese.Add(e2);
                    if (e1.ContainsComponent<Shared.Components.Growable>())
                    {
                        Shared.Components.Growable growthComponent = e1.GetComponent<Shared.Components.Growable>();
                        growthComponent.growth += consumable.growth;
                    }
                }
            }
        }
    }
}
