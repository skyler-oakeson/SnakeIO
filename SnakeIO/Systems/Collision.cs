using System;
using Microsoft.Xna.Framework;

namespace Systems
{
    /// <summary>
    /// This system is responsible for handling the collision of any
    /// entity with Collidable, & Positionable components.
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
            foreach (var e1 in entities.Values)
            {
                foreach (var e2 in entities.Values)
                {
                    bool res = DidCollide(e1, e2);
                    Components.Collidable e1Col = e1.GetComponent<Components.Collidable>();
                    Components.Collidable e2Col = e2.GetComponent<Components.Collidable>();
                    if (res != e1Col.Collided || res != e2Col.Collided)
                    {
                        e1Col.Collided = res;
                        e2Col.Collided = res;
                        if (res) HandleCollision(e1, e2);
                    }
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
            double hitDist = Math.Pow(e1Col.HitBox.Z + e2Col.HitBox.Z, 2);
            double dist = Math.Pow(Math.Abs(e1Col.HitBox.X - e2Col.HitBox.X), 2) + Math.Pow(Math.Abs(e1Col.HitBox.Y - e2Col.HitBox.Y), 2);

            if (dist < hitDist)
            {
                return true;
            }

            return false;
        }

        private void HandleCollision(Entities.Entity e1, Entities.Entity e2)
        {
            Components.Positionable e1Pos = e1.GetComponent<Components.Positionable>();
            Components.Positionable e2Pos = e1.GetComponent<Components.Positionable>();
            e1Pos.Pos = e1Pos.PrevPos;

            //Movables - Non-Movables
            if (e1.ContainsComponent<Components.Movable>() && !e2.ContainsComponent<Components.Movable>())
            {
                Components.Movable e1Mov = e1.GetComponent<Components.Movable>();
                e1Mov.Velocity = -e1Mov.Velocity;
            }

            //Movables - Movables
            if (e1.ContainsComponent<Components.Movable>() && e2.ContainsComponent<Components.Movable>())
            {
                Components.Movable e1Mov = e1.GetComponent<Components.Movable>();
                Components.Movable e2Mov = e2.GetComponent<Components.Movable>();
                e2Mov.Velocity = e1Mov.Velocity;
                e1Mov.Velocity = -e1Mov.Velocity;
            }
        }
    }
}
