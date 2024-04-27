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
        private Dictionary<uint, TimeSpan> invincibleTimes = new Dictionary<uint, TimeSpan>();
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
                    InitializeInvincibility(e1, e2);
                    // if e1 and e2 are not movable, we should not check
                    bool shouldCheck = !(!e1.ContainsComponent<Shared.Components.Movable>() && !e2.ContainsComponent<Shared.Components.Movable>());
                    //Make sure no one is Invincible
                    bool isInvincible = IsInvincible(e1, e2);
                    if (shouldCheck && !isInvincible)
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
                            HandleCollision(e1, e2, elapsedTime);
                        }
                    }
                }
            }
            foreach (Shared.Entities.Entity entity in removeThese)
            {
                removeEntity(entity);
            }
            foreach (var id in invincibleTimes.Keys)
            {
                invincibleTimes[id] -= elapsedTime;
                entities[id].GetComponent<Shared.Components.Invincible>().time = invincibleTimes[id];
                Shared.Messages.UpdateEntity message = new Shared.Messages.UpdateEntity(entities[id], elapsedTime);
                Server.MessageQueueServer.instance.broadcastMessageWithLastId(message);
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

        private void HandleCollision(Shared.Entities.Entity e1, Shared.Entities.Entity e2, TimeSpan elapsedTime)
        {
            Shared.Components.Positionable e1Pos = e1.GetComponent<Shared.Components.Positionable>();
            Shared.Components.Positionable e2Pos = e2.GetComponent<Shared.Components.Positionable>();
            Vector2 n = (e1Pos.pos - e2Pos.pos);
            n.Normalize();

            int? e1SnakeId = null;
            int? e2SnakeId = null;
            if (e1.ContainsComponent<Shared.Components.SnakeID>())
            {
                e1SnakeId = e1.GetComponent<Shared.Components.SnakeID>().id;
                Server.MessageQueueServer.instance.sendMessage((int)e1SnakeId, new Shared.Messages.Collision(e1, e2));
            }
            if (e2.ContainsComponent<Shared.Components.SnakeID>())
            {
                e2SnakeId = e2.GetComponent<Shared.Components.SnakeID>().id;
                Server.MessageQueueServer.instance.sendMessage((int)e2SnakeId, new Shared.Messages.Collision(e1, e2));
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
                    Shared.Entities.Entity particle = Shared.Entities.Particle.Create(e1SnakeId, "", new Rectangle((int)e1Pos.pos.X, (int)e1Pos.pos.Y, 0, 0), Color.Green, Shared.Components.ParticleComponent.ParticleType.PlayerDeathParticle, e1Pos.orientation);
                    addEntity(particle);

                    RemoveSnake(e2);
                    Server.MessageQueueServer.instance.sendMessage(e2.GetComponent<Shared.Components.SnakeID>().id, new Shared.Messages.GameOver());
                    particle = Shared.Entities.Particle.Create(e2SnakeId, "", new Rectangle((int)e2Pos.pos.X, (int)e2Pos.pos.Y, 0, 0), Color.Green, Shared.Components.ParticleComponent.ParticleType.PlayerDeathParticle, e2Pos.orientation);
                    addEntity(particle);
                    if (e2.ContainsComponent<Shared.Components.KillCount>())
                    {
                        e2.GetComponent<Shared.Components.KillCount>().UpdateCount();
                        Shared.Messages.UpdateEntity message = new Shared.Messages.UpdateEntity(e2, elapsedTime);
                        Server.MessageQueueServer.instance.broadcastMessageWithLastId(message);
                    }
                    if (e1.ContainsComponent<Shared.Components.KillCount>())
                    {
                        e1.GetComponent<Shared.Components.KillCount>().UpdateCount();
                        Shared.Messages.UpdateEntity message = new Shared.Messages.UpdateEntity(e1, elapsedTime);
                        Server.MessageQueueServer.instance.broadcastMessageWithLastId(message);
                    }
                }
                else if (e1Linkable.chain != e2Linkable.chain && (e1.ContainsComponent<Shared.Components.SnakeID>() || e2.ContainsComponent<Shared.Components.SnakeID>()))
                {
                    // Ensure it isn't itself
                    // Find the head of the snake by checking which one has SnakeID
                    Shared.Entities.Entity snake = e1.ContainsComponent<Shared.Components.SnakeID>() ? e1 : e2;
                    RemoveSnake(snake);
                    Server.MessageQueueServer.instance.sendMessage(snake.GetComponent<Shared.Components.SnakeID>().id, new Shared.Messages.GameOver());
                    Shared.Components.Positionable snakePos = snake.GetComponent<Shared.Components.Positionable>();
                    Shared.Components.SnakeID id = snake.GetComponent<Shared.Components.SnakeID>();
                    Shared.Entities.Entity particle = Shared.Entities.Particle.Create(id.id, "", new Rectangle((int)snakePos.pos.X, (int)snakePos.pos.Y, 0, 0), Color.Green, Shared.Components.ParticleComponent.ParticleType.PlayerDeathParticle, snakePos.orientation);
                    if (e1.ContainsComponent<Shared.Components.SnakeID>())
                    {
                        if (e2.ContainsComponent<Shared.Components.KillCount>())
                        {
                            e2.GetComponent<Shared.Components.KillCount>().UpdateCount();
                            Shared.Messages.UpdateEntity message = new Shared.Messages.UpdateEntity(e2, elapsedTime);
                            Server.MessageQueueServer.instance.broadcastMessageWithLastId(message);
                        }
                    }
                    if (e2.ContainsComponent<Shared.Components.SnakeID>())
                    {
                        if (e1.ContainsComponent<Shared.Components.KillCount>())
                        {
                            e1.GetComponent<Shared.Components.KillCount>().UpdateCount();
                            Shared.Messages.UpdateEntity message = new Shared.Messages.UpdateEntity(e1, elapsedTime);
                            Server.MessageQueueServer.instance.broadcastMessageWithLastId(message);
                        }
                    }
                }
            }
            else if (e1.ContainsComponent<Shared.Components.Consumable>() || e2.ContainsComponent<Shared.Components.Consumable>())
            {
                // Hits Consumable
                Shared.Entities.Entity food = e1.ContainsComponent<Shared.Components.Consumable>() ? e1 : e2;
                Server.MessageQueueServer.instance.broadcastMessage(new Shared.Messages.RemoveEntity(food.id));
                Shared.Components.Appearance appearance = food.GetComponent<Shared.Components.Appearance>();
                Shared.Components.Positionable foodPos = food.GetComponent<Shared.Components.Positionable>();
                Shared.Entities.Entity particle = Shared.Entities.Particle.Create(null, "Images/glow", new Rectangle((int)e1Pos.pos.X, (int)e1Pos.pos.Y, 0, 0), appearance.color, Shared.Components.ParticleComponent.ParticleType.FoodParticle);
                addEntity(particle);
                removeThese.Add(food);
                if (e1.ContainsComponent<Shared.Components.Growable>())
                {
                    Shared.Components.Growable growthComponent = e1.GetComponent<Shared.Components.Growable>();
                    growthComponent.UpdateGrowth(food.GetComponent<Shared.Components.Consumable>().growth);
                }
                if (e2.ContainsComponent<Shared.Components.Growable>())
                {
                    Shared.Components.Growable growthComponent = e2.GetComponent<Shared.Components.Growable>();
                    growthComponent.UpdateGrowth(food.GetComponent<Shared.Components.Consumable>().growth);
                }
            }
            else
            {
                // Hits wall
                Shared.Entities.Entity currEntity = e1.ContainsComponent<Shared.Components.Linkable>() ? e1 : e2;
                Shared.Components.Positionable currEntityPos = currEntity.GetComponent<Shared.Components.Positionable>();
                if (currEntity.ContainsComponent<Shared.Components.SnakeID>())
                {
                    int snakeId = currEntity.GetComponent<Shared.Components.SnakeID>().id;
                    Server.MessageQueueServer.instance.sendMessage(snakeId, new Shared.Messages.GameOver());
                    Shared.Entities.Entity particle = Shared.Entities.Particle.Create(snakeId, "Images/death", new Rectangle((int)currEntityPos.pos.X, (int)currEntityPos.pos.Y, 0, 0), Color.Red, Shared.Components.ParticleComponent.ParticleType.PlayerDeathParticle, currEntityPos.orientation);
                    addEntity(particle);
                    RemoveSnake(currEntity);
                }
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
                addEntity(food);
                Server.MessageQueueServer.instance.broadcastMessage(new Shared.Messages.NewEntity(food));
                Server.MessageQueueServer.instance.broadcastMessage(new Shared.Messages.RemoveEntity(snake.id));
            }
        }

        private bool IsInvincible(Shared.Entities.Entity e1, Shared.Entities.Entity e2)
        {
            if (e1.ContainsComponent<Shared.Components.Invincible>())
            {
                Shared.Components.Invincible e1Invincible = e1.GetComponent<Shared.Components.Invincible>();
                if (invincibleTimes[e1.id] < TimeSpan.Zero)
                {
                    invincibleTimes.Remove(e1.id);
                    e1.Remove<Shared.Components.Invincible>();
                }
            }
            if (e2.ContainsComponent<Shared.Components.Invincible>())
            {
                Shared.Components.Invincible e2Invincible = e2.GetComponent<Shared.Components.Invincible>();
                if (invincibleTimes[e2.id] < TimeSpan.Zero)
                {
                    invincibleTimes.Remove(e2.id);
                    e2.Remove<Shared.Components.Invincible>();
                }
            }
            return (e1.ContainsComponent<Shared.Components.Invincible>() || e2.ContainsComponent<Shared.Components.Invincible>());
        }

        private void InitializeInvincibility(Shared.Entities.Entity e1, Shared.Entities.Entity e2)
        {
            if (e1.ContainsComponent<Shared.Components.Invincible>())
            {
                Shared.Components.Invincible e1Invincible = e1.GetComponent<Shared.Components.Invincible>();
                if (!invincibleTimes.ContainsKey(e1.id))
                {
                    invincibleTimes[e1.id] = e1Invincible.time;
                }
            }
            if (e2.ContainsComponent<Shared.Components.Invincible>())
            {
                Shared.Components.Invincible e2Invincible = e2.GetComponent<Shared.Components.Invincible>();
                if (!invincibleTimes.ContainsKey(e2.id))
                {
                    invincibleTimes[e2.id] = e2Invincible.time;
                }
            }
        }
    }
}
