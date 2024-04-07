using System;
using Microsoft.Xna.Framework;

namespace Systems
{
    /// <summary>
    /// This system is responsible for handling the collision of any
    /// entities with Collidable, & Positionable components.
    /// This system handles entites with Movable components (Not Required).
    /// </summary>
    public class Collision : System
    {
        public Collision()
            : base(
                    typeof(Components.Collidable),
                    typeof(Components.Positionable)
                    )
        {
        }

        public override void Update(GameTime gameTime)
        {
            Entities.Entity[] entityArr = new Entities.Entity[entities.Values.Count];
            entities.Values.CopyTo(entityArr, 0);
            for (int i = 0; i < entityArr.Length; i++)
            {
                Entities.Entity e1 = entityArr[i];
                for (int j = i; j < entityArr.Length; j++)
                {
                    Entities.Entity e2 = entityArr[j];
                    bool res = DidCollide(e1, e2);
                    if (res) HandleCollision(e1, e2);
                }
            }
        }

        private bool DidCollide(Entities.Entity e1, Entities.Entity e2)
        {
            if (e1 == e2)
            {
                return false;
            }

            Components.Collidable e1Col = e1.GetComponent<Components.Collidable>();
            Components.Collidable e2Col = e2.GetComponent<Components.Collidable>();
            double hitDist = Math.Pow(e1Col.hitBox.Z + e2Col.hitBox.Z, 2);
            double dist = Math.Pow(Math.Abs(e1Col.hitBox.X - e2Col.hitBox.X), 2) + Math.Pow(Math.Abs(e1Col.hitBox.Y - e2Col.hitBox.Y), 2);

            if (dist < hitDist)
            {
                return true;
            }

            return false;
        }

        private void HandleCollision(Entities.Entity e1, Entities.Entity e2)
        {
            Components.Positionable e1Pos = e1.GetComponent<Components.Positionable>();
            Components.Positionable e2Pos = e2.GetComponent<Components.Positionable>();
            Vector2 n = (e1Pos.pos - e2Pos.pos);
            // n.Normalize();

            e1Pos.pos = e1Pos.prevPos;
            e2Pos.pos = e2Pos.prevPos;

            if (e1.ContainsComponent<Components.Audible>())
            {
                e1.GetComponent<Components.Audible>().play = true;
            }

            // Movables - Non-Movables
            if (e1.ContainsComponent<Components.Movable>() && !e2.ContainsComponent<Components.Movable>())
            {
                Components.Movable e1Mov = e1.GetComponent<Components.Movable>();
                e1Mov.velocity += n;
            }

            // Movables - Movables
            if (e1.ContainsComponent<Components.Movable>() && e2.ContainsComponent<Components.Movable>())
            {
                Components.Movable e1Mov = e1.GetComponent<Components.Movable>();
                Components.Movable e2Mov = e2.GetComponent<Components.Movable>();
                e2Mov.velocity -= n*.5f;
                e1Mov.velocity += n*.5f;
            }
        }
    }
}
