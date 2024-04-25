using Microsoft.Xna.Framework;
using System.Threading;

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
        private Server.GameModel.AddDelegate addEntity;
        public Collision(Server.GameModel.AddDelegate addEntity, Server.GameModel.RemoveDelegate removeEntity)
            : base(
                    typeof(Shared.Components.Collidable),
                    typeof(Shared.Components.Positionable)
                    )
        {
            this.addEntity = addEntity;
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
                            // Thread collisionThread = new Thread(() => HandleCollision(e1, e2));
                            // collisionThread.Start();
                            HandleCollision(e1, e2);
                        }
                    }
                }
            }
            foreach (Shared.Entities.Entity entity in removeThese)
            {
                removeEntity(entity);
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
            Shared.Components.Positionable e1Pos = e1.GetComponent<Shared.Components.Positionable>();
            Shared.Components.Positionable e2Pos = e2.GetComponent<Shared.Components.Positionable>();
            Vector2 n = (e1Pos.pos - e2Pos.pos);
            n.Normalize();

            int snakeId = e1.ContainsComponent<Shared.Components.SnakeID>() ? e1.GetComponent<Shared.Components.SnakeID>().id : e2.GetComponent<Shared.Components.SnakeID>().id;
            Server.MessageQueueServer.instance.sendMessage(snakeId, new Shared.Messages.Collision(e1, e2));

            // Movables - Movables
            if (e1.ContainsComponent<Shared.Components.Movable>() && e2.ContainsComponent<Shared.Components.Movable>())
            {
                Shared.Components.Movable e1Mov = e1.GetComponent<Shared.Components.Movable>();
                Shared.Components.Movable e2Mov = e2.GetComponent<Shared.Components.Movable>();
            }

            if (e1.ContainsComponent<Shared.Components.Linkable>() && e2.ContainsComponent<Shared.Components.Linkable>())
            {
                // Hits another snake
                // Heads both collide, kill everyone
                Shared.Components.Linkable e1Linkable = e1.GetComponent<Shared.Components.Linkable>();
                Shared.Components.Linkable e2Linkable = e2.GetComponent<Shared.Components.Linkable>();
                if (e1.ContainsComponent<Shared.Components.SnakeID>() && e2.ContainsComponent<Shared.Components.SnakeID>())
                {
                    RemoveSnake(e1);
                    Server.MessageQueueServer.instance.sendMessage(e1.GetComponent<Shared.Components.SnakeID>().id, new Shared.Messages.GameOver());
                    Shared.Entities.Entity particle = Shared.Entities.Particle.Create("Images/death", new Rectangle((int)e1Pos.pos.X, (int)e1Pos.pos.Y, 0, 0), Color.Green, Shared.Components.ParticleComponent.ParticleType.PlayerDeathParticle, e1Pos.orientation);
                    addEntity(particle);

                    RemoveSnake(e2);
                    Server.MessageQueueServer.instance.sendMessage(e2.GetComponent<Shared.Components.SnakeID>().id, new Shared.Messages.GameOver());
                    particle = Shared.Entities.Particle.Create("Images/death", new Rectangle((int)e2Pos.pos.X, (int)e2Pos.pos.Y, 0, 0), Color.Green, Shared.Components.ParticleComponent.ParticleType.PlayerDeathParticle, e2Pos.orientation);
                    addEntity(particle);
                }
                else if (e1Linkable.chain != e2Linkable.chain)
                {
                    // Ensure it isn't itself
                    // Find the head of the snake by checking which one has SnakeID
                    Shared.Entities.Entity snake = e1.ContainsComponent<Shared.Components.SnakeID>() ? e1 : e2;
                    RemoveSnake(snake);
                    Server.MessageQueueServer.instance.sendMessage(e2.GetComponent<Shared.Components.SnakeID>().id, new Shared.Messages.GameOver());
                    Shared.Components.Positionable snakePos = snake.GetComponent<Shared.Components.Positionable>();
                    Shared.Entities.Entity particle = Shared.Entities.Particle.Create("Images/death", new Rectangle((int)snakePos.pos.X, (int)snakePos.pos.Y, 0, 0), Color.Green, Shared.Components.ParticleComponent.ParticleType.PlayerDeathParticle, snakePos.orientation);
                }
            }
            else if (e1.ContainsComponent<Shared.Components.Consumable>() || e2.ContainsComponent<Shared.Components.Consumable>())
            {
                // Hits Consumable
                if (e1.ContainsComponent<Shared.Components.Consumable>())
                {
                    Shared.Components.Consumable consumable = e1.GetComponent<Shared.Components.Consumable>();
                    Server.MessageQueueServer.instance.broadcastMessage(new Shared.Messages.RemoveEntity(e1.id));
                    Shared.Components.Appearance appearance = e1.GetComponent<Shared.Components.Appearance>();
                    Shared.Entities.Entity particle = Shared.Entities.Particle.Create("Images/glow", new Rectangle((int)e1Pos.pos.X, (int)e1Pos.pos.Y, 0, 0), appearance.color, Shared.Components.ParticleComponent.ParticleType.FoodParticle);
                    addEntity(particle);
                    removeThese.Add(e1);
                    if (e2.ContainsComponent<Shared.Components.Growable>())
                    {
                        Shared.Components.Growable growthComponent = e2.GetComponent<Shared.Components.Growable>();
                        growthComponent.UpdateGrowth(consumable.growth);
                    }
                }
                else
                {
                    Shared.Components.Consumable consumable = e2.GetComponent<Shared.Components.Consumable>();
                    Shared.Components.Appearance appearance = e2.GetComponent<Shared.Components.Appearance>();
                    Server.MessageQueueServer.instance.broadcastMessage(new Shared.Messages.RemoveEntity(e2.id));
                    Shared.Entities.Entity particle = Shared.Entities.Particle.Create("Images/glow", new Rectangle((int)e1Pos.pos.X, (int)e1Pos.pos.Y, 0, 0), appearance.color, Shared.Components.ParticleComponent.ParticleType.FoodParticle);
                    addEntity(particle);
                    removeThese.Add(e2);
                    if (e1.ContainsComponent<Shared.Components.Growable>())
                    {
                        Shared.Components.Growable growthComponent = e1.GetComponent<Shared.Components.Growable>();
                        growthComponent.UpdateGrowth(consumable.growth);
                    }
                }
            }
            else
            {
                // Hits wall
                Shared.Entities.Entity currEntity = e1.ContainsComponent<Shared.Components.Linkable>() ? e1 : e2;
                Shared.Components.Positionable currEntityPos = currEntity.GetComponent<Shared.Components.Positionable>();
                Server.MessageQueueServer.instance.sendMessage(snakeId, new Shared.Messages.GameOver());
                Shared.Entities.Entity particle = Shared.Entities.Particle.Create("Images/death", new Rectangle((int)currEntityPos.pos.X, (int)currEntityPos.pos.Y, 0, 0), Color.Red, Shared.Components.ParticleComponent.ParticleType.PlayerDeathParticle, currEntityPos.orientation);
                addEntity(particle);
                RemoveSnake(currEntity);
            }
        }

        private void RemoveSnake(Shared.Entities.Entity snake)
        {
            while (!removeThese.Contains(snake))
            {
                removeThese.Add(snake);
                snake = snake.GetComponent<Shared.Components.Linkable>().prevEntity;
                Shared.Components.Positionable snakePos = snake.GetComponent<Shared.Components.Positionable>();
                Shared.Entities.Entity food = Shared.Entities.Food.Create("Images/food", new Rectangle((int)snakePos.pos.X, (int)snakePos.pos.Y, 32, 32));
                Server.MessageQueueServer.instance.broadcastMessage(new Shared.Messages.NewEntity(food));
                Server.MessageQueueServer.instance.broadcastMessage(new Shared.Messages.RemoveEntity(snake.id));
            }
        }
    }
}
