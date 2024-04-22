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
                Shared.Entities.Entity e1 = entityArr[i];
                for (int j = i; j < entityArr.Length; j++)
                {
                    Shared.Entities.Entity e2 = entityArr[j];
                    // if e1 and e2 are not movable, we should not check
                    bool shouldCheck = !(!e1.ContainsComponent<Shared.Components.Movable>() && !e2.ContainsComponent<Shared.Components.Movable>());
                    if (shouldCheck)
                    {
                        bool e1IsCircle = (e1.GetComponent<Shared.Components.Collidable>().Data.Shape == Shared.Components.CollidableShape.Circle);
                        bool e2IsCircle = (e1.GetComponent<Shared.Components.Collidable>().Data.Shape == Shared.Components.CollidableShape.Circle);
                        bool res = false;
                        if (e1IsCircle && e2IsCircle)
                        {
                            res = CircleCollision(e1, e2);
                        }
                        else if (!e1IsCircle || !e2IsCircle)
                        {
                            res = RectangleCircleCollision(e1, e2);
                        }
                        if (res)
                        {
                            HandleCollision(e1, e2);
                        }
                    }
                }
            }
        }

        private bool CircleCollision(Shared.Entities.Entity e1, Shared.Entities.Entity e2)
        {
            if (e1 == e2)
            {
                return false;
            }

            Shared.Components.Collidable e1Col = e1.GetComponent<Shared.Components.Collidable>();
            Shared.Components.Collidable e2Col = e2.GetComponent<Shared.Components.Collidable>();
            double hitDist = Math.Pow(e1Col.Data.CircleData.radius + e2Col.Data.CircleData.radius, 2);
            double dist = Math.Pow(Math.Abs(e1Col.Data.CircleData.x - e2Col.Data.CircleData.x), 2) + Math.Pow(Math.Abs(e1Col.Data.CircleData.y - e2Col.Data.CircleData.y), 2);

            if (dist < hitDist)
            {
                return true;
            }

            return false;
        }

        private bool RectangleCircleCollision(Shared.Entities.Entity e1, Shared.Entities.Entity e2)
        {
            Shared.Components.Collidable e1Col = e1.GetComponent<Shared.Components.Collidable>();
            Shared.Components.Collidable e2Col = e2.GetComponent<Shared.Components.Collidable>();
            Shared.Components.CircleData circleHitBox = e1Col.Data.Shape == Shared.Components.CollidableShape.Circle ? e1Col.Data.CircleData : e2Col.Data.CircleData;
            Shared.Components.RectangleData rectangleHitBox = e1Col.Data.Shape == Shared.Components.CollidableShape.Rectangle ? e1Col.Data.RectangleData : e2Col.Data.RectangleData;

            double circleDistanceX = Math.Abs(circleHitBox.x - rectangleHitBox.y);
            double circleDistanceY = Math.Abs(circleHitBox.y - rectangleHitBox.x);
            if (circleDistanceX > ((rectangleHitBox.width) / 2 + circleHitBox.radius))
            {
                return false;
            }
            if (circleDistanceY > ((rectangleHitBox.height) / 2 + circleHitBox.radius))
            {
                return false;
            }
            if (circleDistanceX <= ((rectangleHitBox.width) / 2))
            {
                return true;
            }
            if (circleDistanceY <= ((rectangleHitBox.height) / 2))
            {
                return true;
            }
            double cornerDistanceSQ = (circleDistanceX - (rectangleHitBox.width) / 2) * (circleDistanceX - (rectangleHitBox.width) / 2) + (circleDistanceY - (rectangleHitBox.height) / 2) * (circleDistanceY - (rectangleHitBox.height) / 2);
            return (cornerDistanceSQ <= (circleHitBox.radius * circleHitBox.radius));
        }

        private void HandleCollision(Shared.Entities.Entity e1, Shared.Entities.Entity e2)
        {
            Console.WriteLine("handle collision");
            if (e1.ContainsComponent<Shared.Components.Audible>())
            {
                e1.GetComponent<Shared.Components.Audible>().play = true;
            }
            else if (e2.ContainsComponent<Shared.Components.Audible>())
            {
                e2.GetComponent<Shared.Components.Audible>().play = true;
            }
        }
    }
}
