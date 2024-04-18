using System;
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
        public Collision()
            : base(
                    typeof(Shared.Components.Collidable),
                    typeof(Shared.Components.Positionable)
                    )
        {
        }

        public override void Update(TimeSpan elapsedTime)
        {
            Shared.Entities.Entity[] entityArr = new Shared.Entities.Entity[entities.Values.Count];
            entities.Values.CopyTo(entityArr, 0);
            for (int i = 0; i < entityArr.Length; i++)
            {
                Console.WriteLine("here");
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
            Console.WriteLine(e1Col.hitBox.Z);

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

            e1Pos.pos = e1Pos.prevPos;
            e2Pos.pos = e2Pos.prevPos;
            Console.WriteLine("HandleCollision");

            if (e1.ContainsComponent<Shared.Components.Audible>())
            {
                e1.GetComponent<Shared.Components.Audible>().play = true;
            }

            // Movables - Non-Movables
            if (e1.ContainsComponent<Shared.Components.Movable>() && !e2.ContainsComponent<Shared.Components.Movable>())
            {
                Shared.Components.Movable e1Mov = e1.GetComponent<Shared.Components.Movable>();
                e1Mov.velocity += n;
            }

            // Movables - Movables
            if (e1.ContainsComponent<Shared.Components.Movable>() && e2.ContainsComponent<Shared.Components.Movable>())
            {
                Shared.Components.Movable e1Mov = e1.GetComponent<Shared.Components.Movable>();
                Shared.Components.Movable e2Mov = e2.GetComponent<Shared.Components.Movable>();
                e2Mov.velocity -= n*.5f;
                e1Mov.velocity += n*.5f;
            }

            if (e1.ContainsComponent<Shared.Components.Consumable>() || e2.ContainsComponent<Shared.Components.Consumable>())
            {
                Console.WriteLine("Consumable hit!");
                if (e1.ContainsComponent<Shared.Components.Consumable>())
                {
                    Shared.Components.Consumable consumable = e1.GetComponent<Shared.Components.Consumable>();
                    SnakeIO.MessageQueueClient.instance.sendMessage(new Shared.Messages.RemoveEntity(e1.id));
                }
                else
                {
                    Shared.Components.Consumable consumable = e2.GetComponent<Shared.Components.Consumable>();
                    SnakeIO.MessageQueueClient.instance.sendMessage(new Shared.Messages.RemoveEntity(e2.id));

                }
            }
        }
    }
}
