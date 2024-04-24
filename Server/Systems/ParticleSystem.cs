using Microsoft.Xna.Framework;
using System.Threading;

namespace Systems
{
    public class ParticleSystem : Shared.Systems.System
    {
        private int sizeMean { get; set; }
        private int sizeStdDev { get; set; }
        private float speedMean { get; set; }
        private float speedStdDev { get; set; }
        private float lifetimeMean { get; set; }
        private float lifetimeStdDev { get; set; }
        private Shared.MyRandom rand = new Shared.MyRandom();

        private Server.GameModel.AddDelegate addEntity;
        private Server.GameModel.RemoveDelegate removeEntity;
        private List<Shared.Entities.Entity> entitiesToAdd = new List<Shared.Entities.Entity>();
        private List<Shared.Entities.Entity> entitiesToRemove = new List<Shared.Entities.Entity>();
        public ParticleSystem(Server.GameModel.AddDelegate addEntity, Server.GameModel.RemoveDelegate removeEntity)
            : base(
                    typeof(Shared.Components.ParticleComponent),
                    typeof(Shared.Components.Appearance)
                    )
        {
            this.addEntity = addEntity;
            this.removeEntity = removeEntity;
        }

        public override void Update(TimeSpan elapsedTime)
        {
            foreach (var entity in entities.Values)
            {
                Shared.Components.ParticleComponent pComponent = entity.GetComponent<Shared.Components.ParticleComponent>();
                if (!pComponent.Update(elapsedTime))
                {
                    entitiesToRemove.Add(entity);
                }
                if (pComponent.shouldCreate && pComponent.type == Shared.Components.ParticleComponent.ParticleType.FoodParticle)
                {
                    EatFood(entity);
                }
                else if (pComponent.shouldCreate && pComponent.type == Shared.Components.ParticleComponent.ParticleType.EnemyDeathParticle)
                {
                    EnemyDeath(entity);
                }
                else if (pComponent.shouldCreate && pComponent.type == Shared.Components.ParticleComponent.ParticleType.PlayerDeathParticle)
                {
                    PlayerDeath(entity);
                }
            }

            foreach (var entity in entitiesToAdd)
            {
                addEntity(entity);
                //TODO: broadcast message
            }

            foreach (var entity in entitiesToRemove)
            {
                removeEntity(entity);
                //TODO: broadcast message
            }
        }

        private Shared.Entities.Entity CreateParticle(Vector2 direction, Shared.Entities.Entity entity)
        {
            float size = (float)rand.nextGaussian(sizeMean, sizeStdDev);
            Shared.Components.ParticleComponent pComponent = entity.GetComponent<Shared.Components.ParticleComponent>();
            Shared.Components.Appearance appearance = entity.GetComponent<Shared.Components.Appearance>();
            Shared.Entities.Entity p = Shared.Entities.Particle.Create(
                    appearance.texturePath,
                    appearance.rectangle,
                    appearance.color,
                    pComponent.type,
                    pComponent.center,
                    direction,
                    (float)rand.nextGaussian(speedMean, speedStdDev),
                    new Vector2(size, size),
                    new TimeSpan(0, 0, 0, 0, (int)rand.nextGaussian(lifetimeMean, lifetimeStdDev))
                    );
            return p;
        }

        private void PlayerDeath(Shared.Entities.Entity entity)
        {

        }

        private void EnemyDeath(Shared.Entities.Entity entity)
        {

        }

        private void EatFood(Shared.Entities.Entity entity)
        {
            //TODO: set values (stdDevs, means, etc.)
            for (int i = 0; i < 30; i++)
            {
                Shared.Entities.Entity particle = CreateParticle(rand.nextCircleVector(), entity);
            }
        }
    }
}
